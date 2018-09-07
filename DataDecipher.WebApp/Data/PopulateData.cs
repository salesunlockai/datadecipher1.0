using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDecipher.WebApp.Models;

namespace DataDecipher.WebApp.Data
{
    public class PopulateDemoData
    {
        public static async Task Initialize(ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<ApplicationRole> roleManager)
        {
            context.Database.EnsureCreated();

            string ddAdminRole = "DD Admin";
            string ddAdminRoleDesc = "This is the administrator role for Data Decipher";

            string superuserRole = "Super User";
            string superuserRoleDesc = "Super user for Data Decipher";

            string userRole = "User";
            string userRoleDesc = "User for Data Decipher";

            string password = "Data1234$$";

           
            if (await roleManager.FindByNameAsync(ddAdminRole) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(ddAdminRole, ddAdminRoleDesc, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(superuserRole) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(superuserRole, superuserRoleDesc, DateTime.Now));
            }
            if (await roleManager.FindByNameAsync(userRole) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(userRole, userRoleDesc, DateTime.Now));
            }

            Organization Ddorganization = new Organization { Name = "Data Decipher" };
            if (context.Organizations.Count() >= 1)
            {
                await context.Organizations.AddAsync(Ddorganization);
            }
          

            Plan FreePlan = new Plan { Name = "Free" };
            if (context.Plans.Count() >= 1)
            {
                await context.Plans.AddAsync(FreePlan);
            }
            if (await userManager.FindByNameAsync("Admin") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@datadecipher.com",
                    Email = "admin@datadecipher.com",
                    FirstName = "Admin",
                    LastName = "@datadecipher",
                    Organization = Ddorganization,
                    Plan = FreePlan
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, ddAdminRole);
                }
            }

            if (await userManager.FindByNameAsync("Superuser") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "superuser@datadecipher.com",
                    Email = "superuser@datadecipher.com",
                    FirstName = "Superuser",
                    LastName = "@datadecipher",
                    Organization = Ddorganization,
                    Plan = FreePlan
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, superuserRole);
                }
            }

            if (await userManager.FindByNameAsync("user") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@datadecipher.com",
                    Email = "user@datadecipher.com",
                    FirstName = "User",
                    LastName = "@datadecipher",
                    Organization = Ddorganization,
                    Plan = FreePlan
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, userRole);
                }
            }

        }
    }
}
