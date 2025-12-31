using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class AuthSeeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await CreateRolesAsync();
            await CreateSuperAdminAsync();
        }

        private async Task CreateRolesAsync()
        {
            List<string> roles = ["SuperAdmin", "Admin", "Editor", "Company", "Candidate"];

            foreach (var role in roles)
            {
                var hasRole = await _roleManager.RoleExistsAsync(role);

                if (hasRole) continue;

                await _roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }

        private async Task CreateSuperAdminAsync()
        {
            var email = "superadmin@jobportal.com";
            if (await _userManager.FindByEmailAsync(email) == null)
            {
                var user = new AppUser
                {
                    UserName = "SuperAdmin",
                    Email = email,
                    EmailConfirmed = true
                };

                // Create user
                var result = await _userManager.CreateAsync(user, "123456");

                // Assign Role
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }
    }
}
