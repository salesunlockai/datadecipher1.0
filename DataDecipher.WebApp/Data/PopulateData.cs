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

            //Seed Dataconnectors
            if (context.DataSourceConnectors.Count() == 0)
            {
                DataSourceConnector xmlConnector = new DataSourceConnector { Name = "XML", Extension = "xml" };
                DataSourceConnector datConnector = new DataSourceConnector { Name = "DAT", Extension = "dat" };
                DataSourceConnector csvConnector = new DataSourceConnector { Name = "CSV", Extension = "csv" };
                DataSourceConnector txtConnector = new DataSourceConnector { Name = "TXT", Extension = "txt" };

                await context.DataSourceConnectors.AddRangeAsync(new DataSourceConnector[] { xmlConnector, datConnector, csvConnector, txtConnector });
                await context.SaveChangesAsync();

            }

            //Seed Plan
            if (context.Plans.Count() == 0)
            {
                Plan freePlan = new Plan { Name = "Free", TrialPeriod=30, Price=0};
                await context.Plans.AddAsync(freePlan);
                await context.SaveChangesAsync();
            }

            //Seed PlanDataConnector
            if (context.PlanDataConnectors.Count() == 0)
            {
                foreach ( DataSourceConnector item in context.DataSourceConnectors)
                {

                    await context.PlanDataConnectors.AddAsync(new PlanDataConnector { Plan = context.Plans.First(), DataSourceConnector = item });
                }
                await context.SaveChangesAsync();
            }

            //Seed Organization
            if (context.Organizations.Count() == 0)
            {
                Organization ddOrganization = new Organization { Name = "Data Decipher", SelectedPlan= context.Plans.First()};
                await context.Organizations.AddAsync(ddOrganization);
                await context.SaveChangesAsync();
            }
          
            //Seed Roles

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

            // Seed Users
            if (await userManager.FindByNameAsync("Admin") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@datadecipher.com",
                    Email = "admin@datadecipher.com",
                    FirstName = "Admin",
                    LastName = "@datadecipher",
                    Organization = context.Organizations.First(),
                    Plan = context.Plans.First()
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
                    Organization = context.Organizations.First(),
                    Plan = context.Plans.First()
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
                    Organization = context.Organizations.First(),
                    Plan = context.Plans.First()
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
