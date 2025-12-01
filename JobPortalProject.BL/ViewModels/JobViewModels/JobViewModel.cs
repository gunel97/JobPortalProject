using JobPortalProject.BL.ViewModels.AddressViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobViewModels
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string DetailsUrl => $"{Title?.Replace(" ", "-").Replace("/", "-")}-{Id}";
        public string? Description { get; set; }
        public int VacancyCount { get; set; }
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }
        public bool IsActive { get; set; }
        public string? Gender { get; set; }
        public string? SalaryTypeDuration { get; set; }
        public string? RequiredMinEducationType { get; set; }
        public string? JobType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int JobCategoryId {get;set;}
        public string? JobCategoryName { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public AddressViewModel? Address { get; set; }
        public List<string> Responsibilities { get; set; } = [];
        public List<string> Benefits { get; set; } = [];
        public List<string> MainDuties { get; set; } = [];
        public List<string> CompanyImages { get; set; } = [];
    }

    public class JobCreateViewModel
    {
    }

    public class JobUpdateViewModel
    {
    }

    public class JobTranslationViewModel
    {
    }

    public class JobTranslationCreateViewModel
    {
    }

    public class JobTranslationUpdateViewModel
    {
    }

}
