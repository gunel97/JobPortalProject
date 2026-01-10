using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class CompanyIndexViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? DefaultName { get; set; }
        public int TotalJobCount { get; set; }
        public int TotalAddressCount { get; set; }
        public int TotalAcceptedApplicationCount { get; set; }
        public AddressViewModel? MainAddress { get; set; }
        public DateTime MemberSince { get; set; }
        public bool IsAccountDeleted { get; set; }
    }

    public class CompanyPagedIndexViewModel
    {
        public PagedResultModel<CompanyIndexViewModel> Companies { get; set; } = null!;
        public CompanyIndexFilterViewModel? Filter { get; set; }
        public List<LanguageViewModel> Languages { get; set; } = [];
    }

    public class CompanyIndexFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "Membersince";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
    }
}
