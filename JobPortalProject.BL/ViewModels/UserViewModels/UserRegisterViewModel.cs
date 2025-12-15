using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.UserViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        public string? UserType { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public  string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }

    public class CompanyRegisterViewModel : UserRegisterViewModel
    {
        [Required(ErrorMessage = "Company name is required")]
        public string? CompanyName { get; set; }
        public int CompanyTypeId { get; set; }
        public List<SelectListItem> CompanyTypesList { get; set; } = [];
    }

    public class LoginViewModel
    {
        public required string Username { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}
