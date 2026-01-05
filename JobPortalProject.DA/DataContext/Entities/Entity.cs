using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public List<JobResponsibility> Responsibilities { get; set; } = [];
        public List<JobExtraBenefit> ExtraBenefits { get; set; } = [];
        public List<JobMainDuty> MainDuties { get; set; } = [];
        public List<JobTranslation> JobTranslations { get; set; } = [];
        public List<JobApplication> JobApplications { get; set; } = [];

    }

    public class JobTranslation : TimeStample
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string RequiredExperience { get; set; } = null!;
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class JobResponsibility : TimeStample
    {
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public List<JobResponsibilityTranslation> JobResponsibilityTranslations { get; set; } = [];
    }

    public class JobResponsibilityTranslation : TimeStample
    {
        public string? Value { get; set; }
        public int JobResponsibilityId { get; set; }
        public JobResponsibility? JobResponsibility { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class JobExtraBenefit : TimeStample
    {
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public List<JobExtraBenefitTranslation> JobExtraBenefitTranslations { get; set; } = [];
    }

    public class JobExtraBenefitTranslation : TimeStample
    {
        public string? Value { get; set; }
        public int JobExtraBenefitId { get; set; }
        public JobExtraBenefit? JobExtraBenefit { get; set; }
        public int LanguageId { get; set; }
        public Language? Language{ get; set; }
    }

    public class JobMainDuty : TimeStample
    {
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public List<JobMainDutyTranslation> JobMainDutyTranslations { get; set; } = [];
    }

    public class JobMainDutyTranslation : TimeStample
    {
        public string? Value { get; set; }
        public int JobMainDutyId { get; set; }
        public JobMainDuty? JobMainDuty { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }

    }

    public class Company : TimeStample
    {
        public int CompanySize { get; set; }
        public string? CoverPhotoPublicId { get; set; } 
        public string? CoverPhotoUrl { get; set; } = null!;
        public string? LogoPublicId { get; set; } = null!;
        public string? LogoUrl { get; set; } = null!;
        public string? PrimaryPhone { get; set; } = null!;
        public string? SecondaryPhone { get; set; } = null!;
        public string? CompanyEmail { get; set; } = null!;
        public bool IsAccountApproved { get; set; }
        public DateTime? MemberSince { get; set; }
        public DateTime? LastPostedJob { get; set; }
        //
        public List<Job> Jobs { get; set; } = [];
        public List<CompanyImage> CompanyImages { get; set; } = [];
        public List<CompanySocial> CompanySocials { get; set; } = [];
        public List<CompanyTranslation> CompanyTranslations { get; set; } = [];
        public List<Address> Addresses { get; set; } = [];
        public List<WorkingField> WorkingFields { get; set; } = [];
        public int CompanyTypeId { get; set; }
        public CompanyType? CompanyType { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public List<Order> Orders { get; set; } = [];
        public DateTime? MembershipExpiresAt { get; set; }
        [NotMapped]
        public bool IsMembershipActive => MembershipExpiresAt.HasValue && MembershipExpiresAt.Value > DateTime.UtcNow;
    }

    public class Candidate:TimeStample
    {
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }
        public Resume? Resume { get; set; }
        public List<JobApplication> JobApplications { get; set; } = [];
    }

    public class PersonalInfo : TimeStample
    {
        public string WorkEmail { get; set; } = null!; 
        public string ImageUrl { get; set; } = null!;
        public string ImagePublicId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int ResumeId { get; set; }
        public Resume? Resume { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public List<PersonalInfoTranslation> Translations { get; set; } = [];
    }

    public class PersonalInfoTranslation : TimeStample
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public int PersonalInfoId {  get; set; }
        public PersonalInfo? PersonalInfo { get; set; }
    }

    public class Resume : TimeStample
    {
        public int CandidateId { get; set; }
        public Candidate? Candidate { get; set; }
        public PersonalInfo? PersonalInfo { get; set; }
        public List<Experience> Experiences { get; set; } = [];
        public List<Education> Educations { get; set; } = [];
        public List<ResumeTranslation> Translations { get; set; } = [];

    }

    public class ResumeTranslation : TimeStample
    {
        public string About { get; set; } = null!;
        public List<string> Skills { get; set; } = [];
        public List<string> Languages { get; set; } = [];
        public int ResumeId { get; set; }
        public Resume? Resume { get; set; }
        public int LanguageId { get; set; }
        public Language? Language {  get; set; }

    }

    public class Experience : TimeStample
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ResumeId { get; set; }
        public Resume? Resume { get; set; }
        public List<ExperienceTranslation> Translations { get; set; } = [];
    }

    public class ExperienceTranslation : TimeStample
    {
        public string CompanyName { get; set; } = null!;
        public string Responsibility { get; set; } = null!;
        public string Position { get; set; } = null!;
        public int ExperienceId { get; set; }
        public Experience? Experience {  get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class Education : TimeStample
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EducationType EducationType { get; set; }
        public int ResumeId { get; set; }
        public Resume? Resume { get; set; }
        public List<EducationTranslation> Translations { get; set; } = [];
    }

    public class EducationTranslation : TimeStample
    {
        public string SchoolName { get; set; } = null!;
        public string MajorName { get; set; } = null!;
        public int EducationId { get; set; }
        public Education? Education { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class CompanyTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class CompanyType : TimeStample
    {
        public List<Company> Companies { get; set; } = [];
        public List<CompanyTypeTranslation> CompanyTypeTranslations { get; set; } = [];
    }

    public class CompanyTypeTranslation : TimeStample
    {
        public string Name { get; set; } = null!;
        public int CompanyTypeId { get; set; }
        public CompanyType? CompanyType { get; set; }
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

    public class WorkingField : TimeStample
    {
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public string IconPublicId { get; set; } = null!;
        public string IconUrl { get; set; }=null!;
        public List<WorkingFieldTranslation> Translations { get; set; } = [];
    }

    public class WorkingFieldTranslation:TimeStample
    {
        public int WorkingFieldId { get; set; }
        public WorkingField? WorkingField { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class SocialMedia : TimeStample
    {
        public string IconPublicId { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
    }

    public class MainSocial : TimeStample
    {
        public string IconPublicId { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Url { get; set; }=null!;
    }

    public class Bio : TimeStample
    {
        public string Phone { get; set; } = null!;
        public string LogoPublicId { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
    }

    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Company? Company { get; set; } 
        public Candidate? Candidate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Country : TimeStample
    {
        public List<City> Cities { get; set; } = [];
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
        public string CoverPhotoPublicId { get; set; } = null!;
        public string CoverPhotoUrl { get; set; } = null!;
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
        public bool IsMainAddress { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public PersonalInfo? PersonalInfo { get; set; }
        public List<AddressTranslation> AddressTranslations { get; set; } = [];
        public List<Job> Jobs { get; set; } = [];
    }

    public class AddressTranslation : TimeStample
    {
        public string Street { get; set; } = null!;
        public int AddressId { get; set; } 
        public Address? Address { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }

    public class JobApplication : TimeStample
    {
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public int CandidateId { get; set; }
        public Candidate? Candidate { get; set; }
        public JobApplicationStatus JobStatus { get; set; } 
    }

    public class Order:Entity
    {
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public PaymentStatus Status { get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? StripeSessionId { get; set; } 
        public string? StripePaymentIntentId { get; set; }
    }

}

