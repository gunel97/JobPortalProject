using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.Settings;
using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class MembershipManager : IMembershipService
    {
        private readonly AppDbContext _dbContext;
        private readonly StripeSettings _settings;

        public MembershipManager(IOptions<StripeSettings> options, AppDbContext dbContext)
        {
            _settings = options.Value;
            _dbContext = dbContext;
            StripeConfiguration.ApiKey = _settings.SecretKey;
        }

        public async Task<string> CreateRenewalCheckoutSessionAsync(int companyId)
        {
            decimal price = 50.00m;

            // 1. Create Pending Order in DB
            var order = new DA.DataContext.Entities.Order
            {
                CompanyId = companyId,
                Amount = price,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // 2. Build Stripe Session
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(price * 100), // 5000 cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Standard Membership (30 Days)",
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                // CRITICAL: Pass OrderId so we know which record to update later
                Metadata = new Dictionary<string, string> { { "OrderId", order.Id.ToString() } },
                SuccessUrl = "https://localhost:7122/Order/Success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7122/Order/Error",
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = await service.CreateAsync(options);

            // 3. Save Stripe Session ID to DB
            order.StripeSessionId = session.Id;
            await _dbContext.SaveChangesAsync();

            return session.Url;
        }


        public async Task<bool> ProcessPaymentSuccessAsync(string sessionId)
        {
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = await service.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                if (session.Metadata.TryGetValue("OrderId", out string orderIdStr))
                {
                    int orderId = int.Parse(orderIdStr);

                    var order = await _dbContext.Orders
                                              .Include(o => o.Company)
                                              .FirstOrDefaultAsync(o => o.Id == orderId);

                    // Idempotency: Only proceed if status is not yet 'Paid'
                    if (order != null && order.Status != PaymentStatus.Paid)
                    {
                        // A. Update Order
                        order.Status = PaymentStatus.Paid;
                        order.StripePaymentIntentId = session.PaymentIntentId;

                        // B. Update Company Membership Dates
                        var company = order.Company;
                        DateTime now = DateTime.UtcNow;

                        // If active, add 30 days to existing date. If expired, add 30 days to NOW.
                        if (company.IsMembershipActive)
                        {
                            company.MembershipExpiresAt = company.MembershipExpiresAt.Value.AddDays(30);
                        }
                        else
                        {
                            company.MembershipExpiresAt = now.AddDays(30);
                        }

                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
