using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IEnumService
    {
        public List<SelectListItem> GetJobTypeListItems();
        public List<SelectListItem> GetGenderListItems();
        public List<SelectListItem> GetSalaryTypeListItems();
        public List<SelectListItem> GetEducationTypeListItems();
        public List<SelectListItem> GetPaymentStatusListItems();
    }
}
