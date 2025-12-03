using JobPortalProject.BL.ViewModels.CompanyViewModels;
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
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? UserType { get; set; }

        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }


        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }

    public class CompanyRegisterViewModel : UserRegisterViewModel
    {
        public required string CompanyName { get; set; }
        public int CompanyTypeId { get; set; }
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
