using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.UserViewModels
{
    public class UserFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
    
    public class UserPagedViewModel
    {
        public PagedResultModel<UserViewModel> Users { get; set; } = null!;
    }

    public class UserViewModel
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserUpdateViewModel
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }= null!;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? ChangePassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Admin password is required")]
        public string AdminPassword { get; set; } = null!;
    }

    public class CompanyUserViewModel : UserViewModel
    {
        public string? CompanyName { get; set; }
        public string? CompanyEmail { get; set; }
        public DateTime LastPostedJob { get; set; }
        public int TotalJobCount { get; set; }
    }
}
