using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<CompanyAddress> CompanyAddresses { get; set; } = null!;
        public DbSet<Bio> Bios { get; set; } = null!;
        public DbSet<MainSocial> MainSocials {get; set;}=null!;
        public DbSet<WorkingField> WorkingFields { get; set; }=null!;
        public DbSet<WorkingFieldTranslation> WorkingFieldTranslations { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .Property(p => p.MinSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Job>()
                .Property(p => p.MaxSalary)
                .HasPrecision(18, 2);


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
