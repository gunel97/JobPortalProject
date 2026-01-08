using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
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
    public class CompanyUserFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
    }
    public class CandidateUserFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
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
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CompanyUserViewModel : UserViewModel
    {
        public string? CompanyName { get; set; }
        public string? CompanyEmail { get; set; }
        public DateTime LastPostedJob { get; set; }
        public int TotalJobCount { get; set; }
    }

    public class CompanyUserPagedViewModel
    {
        public PagedResultModel<CompanyUserViewModel> Users { get; set; } = null!;
    }

    public class CandidateUserViewModel : UserViewModel
    {
        public DateTime BirthDate { get; set; }
    }

}
