namespace HeroesArenaWebsite.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using HeroesArenaWebsite.Common;
    using HeroesArenaWebsite.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);

            //, string adminEmail, string adminUsername, string adminPassword

            ////Check if the admin user exists and create it if not
            ////Add to the Administrator role

            //Task<ApplicationUser> testUser = userManager.FindByEmailAsync(adminEmail);
            //testUser.Wait();

            //if (testUser.Result == null)
            //{
            //    ApplicationUser administrator = new ApplicationUser
            //    {
            //        Email = adminEmail,
            //        UserName = adminUsername,
            //    };

            //    Task<IdentityResult> newUser = userManager.CreateAsync(administrator, adminPassword);
            //    newUser.Wait();

            //    if (newUser.Result.Succeeded)
            //    {
            //        Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Administrator");
            //        newUserRole.Wait();
            //    }
            //}
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
