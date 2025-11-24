using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.DataContext.Entities
{
    public class Entity
    {
        public int Id { get; set; }
    }

    public class TimeStample : Entity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class Language : TimeStample
    {
        public string IconPublicId { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string IsoCode { get; set; } = null!;
    }

    public class JobCategory : TimeStample
    {
        public string ImagePublicId {  get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public List<JobCategoryTranslation> JobCategoryTranslations { get; set; } = [];
        public List<Job> Jobs { get; set; } = [];
    }

    public class JobCategoryTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public int JobCategoryId { get; set; }
        public JobCategory? JobCategory { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }

    }

    public class Job : TimeStample
    {
        public int VacancyCount { get; set; }
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }
        public JobType JobType { get; set; }
        public EducationType RequiredMinEducationType { get; set; }
        public SalaryTypeDuration SalaryTypeDuration { get; set; }
        public Gender Gender { get; set; }
        //
        public int JobCategoryId { get; set; }
        public JobCategory? JobCategory { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public List<JobTranslation> JobTranslations { get; set; } = [];


    }

    public class JobTranslation : TimeStample
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> Responsibilities { get; set; } = [];
        public List<string> ExtraBenefits { get; set; } = [];
        public List<string> MainDuties { get; set; } = [];
        //
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class Company : TimeStample
    {
        public string CoverPhotoPublicId { get; set; } = null!;
        public string CoverPhotoUrl { get; set; } = null!;
        public string LogoPublicId { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string PrimaryPhone { get; set; } = null!;
        public string SecondaryPhone { get; set; } = null!;
        public string Email { get; set; } = null!;
        //
        public List<Job> Jobs { get; set; } = [];
        public List<CompanyImage> CompanyImages { get; set; } = [];
        public List<CompanySocial> CompanySocials { get; set; } = [];
        public List<CompanyTranslation> CompanyTranslations { get; set; } = [];
        public List<CompanyAddress> CompanyAddresses { get; set; } = [];
        public int CompanyTypeId { get; set; }
        public CompanyType? CompanyType { get; set; }
    }

    public class CompanyTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class CompanyType : TimeStample
    {
        public List<Company> Companies { get; set; } = [];
    }

    public class CompanyTypeTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }


    public class CompanySocial : TimeStample
    {
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int SocialMediaId { get; set; }
        public SocialMedia? SocialMedia { get; set; }
        public string AddressUrl { get; set; } = null!;
    }

    public class CompanyImage:TimeStample
    {
        public string ImagePublicId { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }

    public class SocialMedia : TimeStample
    {
        public string IconPublicId { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
    }

    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }


    public class Country : TimeStample
    {
        public List<City> Cities { get; set; } = null!;
        public List<CountryTranslation> Translations { get; set; } = [];

    }

    public class CountryTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class City : TimeStample
    {
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public List<Address> Addresses { get; set; } = [];
        public List<CityTranslation> CityTranslations { get; set; } = [];
    }

    public class CityTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public int CityId { get; set; }
        public City? City { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class Address : TimeStample
    {
        public int CityId { get; set; }
        public City? City { get; set; }
        public List<AddressTranslation> AddressTranslations { get; set; } = [];
        public List<CompanyAddress> CompanyAddresses { get; set; } = [];

    }

    public class AddressTranslation : TimeStample
    {
        public string Street { get; set; } = null!;
        public int AddressId { get; set; } 
        public Address? Address { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class CompanyAddress:TimeStample
    {
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int AddressId { get; set; }
        public Address? Address { get; set; }
    }

}

