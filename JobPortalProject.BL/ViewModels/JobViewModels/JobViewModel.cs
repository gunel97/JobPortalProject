using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string? RequiredExperience {  get; set; }
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
        public List<string> ExtraBenefits { get; set; } = [];
        public List<string> MainDuties { get; set; } = [];
        public List<string> CompanyImages { get; set; } = [];
    }

    public class JobCreateViewModel
    {
        public int VacancyCount { get; set; }
        public int CompanyId { get; set; }
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public List<SelectListItem> GenderListItems { get; set; } = [];
        public int JobTypeId { get; set; }
        public JobType JobType { get; set; }
        public List<SelectListItem> JobTypeListItems { get; set; } = [];
        public int RequiredEducationTypeId { get; set; }
        public EducationType RequiredEducationType { get; set; }
        public List<SelectListItem> RequiredEducationTypeListItems { get; set; } = [];
        public int SalaryTypeId { get; set; }
        public SalaryTypeDuration SalaryType { get; set; }
        public List<SelectListItem> SalaryTypeListItems { get; set; } = [];
        public int AddressId { get; set; }
        public List<SelectListItem> AddressesList { get; set; } = [];
        public int JobCategoryId { get; set; }
        public List<SelectListItem> JobCategoriesList { get; set; } = [];
        public List<JobTranslationCreateViewModel> TranslationCreateViewModels { get; set; } = [];
        public List<JobResponsibilityCreateViewModel> Responsibilities { get; set; } = [];
        public List<JobExtraBenefitCreateViewModel> ExtraBenefits { get; set; } = [];
    }


    public class JobUpdateViewModel
    {
        public int Id { get; set; }
        public int VacancyCount { get; set; }
        public int CompanyId { get; set; }
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public List<SelectListItem> GenderListItems { get; set; } = [];
        public int JobTypeId { get; set; }
        public JobType JobType { get; set; }
        public List<SelectListItem> JobTypeListItems { get; set; } = [];
        public int RequiredEducationTypeId { get; set; }
        public EducationType RequiredMinEducationType { get; set; }
        public List<SelectListItem> RequiredEducationTypeListItems { get; set; } = [];
        public int SalaryTypeId { get; set; }
        public SalaryTypeDuration SalaryType { get; set; }
        public List<SelectListItem> SalaryTypeListItems { get; set; } = [];
        public int AddressId { get; set; }
        public List<SelectListItem> AddressesList { get; set; } = [];
        public int JobCategoryId { get; set; }
        public List<SelectListItem> JobCategoriesList { get; set; } = [];
        public List<JobTranslationUpdateViewModel> JobTranslations { get; set; } = [];
        public List<JobResponsibilityUpdateViewModel> Responsibilities { get; set; } = [];
        public List<JobExtraBenefitUpdateViewModel> ExtraBenefits { get; set; } = [];
    }

    public class JobTranslationViewModel
    {
    }

    public class JobTranslationCreateViewModel
    {
        public int JobId { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get;set; } = null!;
        public string RequiredExperience { get; set; } = null!;
        public string? LanguageIcon { get; set; }
    }

    public class JobTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? RequiredExperience { get; set; }
        public string? LanguageIcon { get; set; }
    }

}
