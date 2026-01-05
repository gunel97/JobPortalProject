using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IMembershipService
    {
        public Task<string> CreateRenewalCheckoutSessionAsync(int companyId);
        public Task<bool> ProcessPaymentSuccessAsync(string sessionId);
    }
}
