using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortalProject.UserMvc.Attributes
{
        public class RequiresMembershipAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                // 1. Get Database Context
                var db = context.HttpContext.RequestServices.GetService<AppDbContext>();

                // 2. Get Logged-in User ID (Identity User ID)
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    // 3. Fetch AppUser AND Include the Company
                    // We use .FirstOrDefault() directly (synchronous is safer inside filters to avoid async deadlock issues)
                    var appUser = db.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.Id == userId);

                    // 4. CHECK: Is this user actually a Company?
                    if (appUser?.Company == null)
                    {
                        // Logic: If user is logged in but has NO Company profile, 
                        // they might be a "Candidate" trying to access a Company page.
                        // We should block them or redirect them to Home.
                        context.Result = new RedirectToActionResult("Index", "Home", null);
                    }
                    else
                    {
                        // 5. CHECK: Is the Membership Active?
                        // We use your exact property: IsMembershipActive
                        if (!appUser.Company.IsMembershipActive)
                        {
                            // STOP the request. Redirect to Payment Checkout.
                            context.Result = new RedirectToActionResult("Checkout", "Order", null);
                        }
                    }
                }
                else
                {
                    // User is not logged in at all
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }

                base.OnActionExecuting(context);
            }
        }
    }

