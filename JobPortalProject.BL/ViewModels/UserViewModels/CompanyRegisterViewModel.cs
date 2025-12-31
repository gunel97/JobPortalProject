using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobPortalProject.BL.ViewModels.UserViewModels
{
    public class CompanyRegisterViewModel : UserRegisterViewModel
    {
        [Required(ErrorMessage = "Company name is required")]
        public string? CompanyName { get; set; }
        public int CompanyTypeId { get; set; }
        public List<SelectListItem> CompanyTypesList { get; set; } = [];
    }
}
