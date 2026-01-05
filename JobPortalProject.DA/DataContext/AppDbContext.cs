using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.DataContext
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SocialMedia> SocialMedias { get; set; } = null!;
        public DbSet<CompanyImage> CompanyImages { get; set;} = null!;
        public DbSet<CompanySocial> CompanySocials { get; set; } = null!;
        public DbSet<CompanyTypeTranslation> CompanyTypeTranslations { get; set; } = null!;
        public DbSet<CompanyType> CompanyTypes { get; set; } = null!;
        public DbSet<CompanyTranslation> CompanyTranslations { get; set; }=null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<JobTranslation> JobTranslations { get; set; } = null!;
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<JobCategoryTranslation> JobCategoryTranslations { get; set; } = null!;
        public DbSet<JobCategory> JobCategories { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<AddressTranslation> AddressTranslation { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<CountryTranslation> CountriesTranslation { get; set; }= null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<CityTranslation> CityTranslations { get; set; } = null!;
        public DbSet<Bio> Bios { get; set; } = null!;
        public DbSet<MainSocial> MainSocials {get; set;}=null!;
        public DbSet<WorkingField> WorkingFields { get; set; }=null!;
        public DbSet<WorkingFieldTranslation> WorkingFieldTranslations { get; set; } = null!;
        public DbSet<JobExtraBenefit> JobExtraBenefits { get; set; }=null!;
        public DbSet<JobExtraBenefitTranslation> JobExtraBenefitTranslations { get; set; }=null!;
        public DbSet<JobResponsibility> JobResponsibilities { get; set; }=null!;
        public DbSet<JobResponsibilityTranslation> JobResponsibilityTranslations { get; set; }=null!;
        public DbSet<JobMainDuty> JobMainDuties { get; set; }=null!;
        public DbSet<JobMainDutyTranslation> JobMainDutyTranslations { get; set; }=null!;
        public DbSet<Candidate> Candidates { get; set; }=null!;
        public DbSet<Resume> Resumes { get; set; } = null!;
        public DbSet<ResumeTranslation> ResumeTranslations { get; set; } = null!;
        public DbSet<PersonalInfo> PersonalInfos { get; set; }=null!;
        public DbSet<PersonalInfoTranslation> PersonalInfoTranslations { get; set; }=null!;
        public DbSet<Experience> Experiences { get; set; }=null!;
        public DbSet<ExperienceTranslation> ExperienceTranslations { get; set; }=null!;
        public DbSet<Education> Educations { get; set; }=null!;
        public DbSet<EducationTranslation> EducationTranslations { get; set; } = null!;
        public DbSet<JobApplication> JobApplications { get; set; }=null!;
        public DbSet<Order> Orders { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .Property(p => p.MinSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Job>()
                .Property(p => p.MaxSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Company>()
        .HasMany(c => c.Addresses)
        .WithOne(a => a.Company)
        .HasForeignKey(a => a.CompanyId)
        .OnDelete(DeleteBehavior.Cascade); // burda qala bilər

            // City – Address (1–çox)
            modelBuilder.Entity<City>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.City)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict); // və ya NoAction

            // Address – Job (1–çox) → BURDA CASCADE OLMASIN
            modelBuilder.Entity<Address>()
                .HasMany(a => a.Jobs)
                .WithOne(j => j.Address)
                .HasForeignKey(j => j.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonalInfo>()
                .HasOne(p => p.Resume)
                .WithOne(r => r.PersonalInfo)
                .HasForeignKey<PersonalInfo>(p => p.ResumeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Resume>()
                .HasOne(r => r.Candidate)
                .WithOne(c => c.Resume)
                .HasForeignKey<Resume>(r => r.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonalInfo>()
                .HasOne(p => p.Address)
                .WithMany()
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobApplication>()
       .HasOne(ja => ja.Job)
       .WithMany(j => j.JobApplications)
       .HasForeignKey(ja => ja.JobId)
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Candidate)
                .WithMany(c => c.JobApplications)
                .HasForeignKey(ja => ja.CandidateId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimeStamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimeStamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is TimeStample &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var timeStamp = (TimeStample)entry.Entity;

                if (entry.State == EntityState.Added)
                    timeStamp.CreatedAt = DateTime.UtcNow.AddHours(4);

                timeStamp.UpdatedAt = DateTime.UtcNow.AddHours(4);
            }
        }
    }
}
