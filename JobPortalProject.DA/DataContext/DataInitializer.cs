using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.DataContext
{
    public class DataInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(AppDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await _dbContext.Database.MigrateAsync();
            await CreateRoles();
        }

        public async Task CreateRoles()
        {
            List<string> roles = ["Admin", "Company", "Candidate"];

            foreach (var role in roles)
            {
                var hasRole = await _roleManager.RoleExistsAsync(role);

                if (hasRole) continue;

                await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }
    }
}
