using DataDecipher.WebApp.Data;
using DataDecipher.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataDecipher.WebApp.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ApplicationUser> users = context.ApplicationUser.Include(x => x.Organization).ToList();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (ApplicationUser x in users)
            {
                string roleName = "";
                var lrole = context.UserRoles.Where(y => y.UserId == x.Id);
                if (lrole.Count() != 0)
                {
                    string roleid = lrole.FirstOrDefault().RoleId;
                    if (roleid != "")
                    {
                        ApplicationRole role = context.ApplicationRole.Find(roleid);
                        roleName = role.Name;
                    }
                }
                keyValuePairs.Add(x.Id, roleName);
            }
            ViewBag.LinkedRole = keyValuePairs;
            return View(users);
        }

        public IActionResult Create()
        {
            ApplicationUserListViewModel model = new ApplicationUserListViewModel();

            model.AvailableOrganizations = context.Organizations.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            model.AvailableRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            return View(model);
        }

        // POST: ApplicationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUserListViewModel applicationUser)
        {
            applicationUser.AvailableOrganizations = context.Organizations.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            applicationUser.AvailableRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            if (ModelState.IsValid)
            {
                if (await userManager.FindByNameAsync(applicationUser.UserName) == null)
                {
                    var result = await userManager.CreateAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        await userManager.AddPasswordAsync(applicationUser, applicationUser.Password);
                        await userManager.AddToRoleAsync(applicationUser, applicationUser.ApplicationRoleId);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(applicationUser);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await context.ApplicationUser.Include(x => x.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            var lrole = context.UserRoles.Where(y => y.UserId == id);
            string roleName = "";
            if (lrole.Count() != 0)
            {
                string roleid = lrole.FirstOrDefault().RoleId;
                if (roleid != "")
                {
                    ApplicationRole role = context.ApplicationRole.Find(roleid);
                    roleName = role.Name;
                }
            }
            ViewBag.LinkedRole = roleName;
            return View(applicationUser);
        }


        // GET: ApplicationUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await context.ApplicationUser.Include(x => x.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            var lrole = context.UserRoles.Where(y => y.UserId == id);
            string roleName = "";
            if (lrole.Count() != 0)
            {
                string roleid = lrole.FirstOrDefault().RoleId;
                if (roleid != "")
                {
                    ApplicationRole role = context.ApplicationRole.Find(roleid);
                    roleName = role.Name;
                }
            }
            ViewBag.LinkedRole = roleName;
            return View(applicationUser);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await context.ApplicationUser.FindAsync(id);
            await userManager.DeleteAsync(applicationUser);
            return RedirectToAction(nameof(Index));
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser applicationUser = await context.ApplicationUser.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            ApplicationUserListViewModel model = new ApplicationUserListViewModel();
            model.UserName = applicationUser.UserName;
            model.Email = applicationUser.Email;
            model.FirstName = applicationUser.FirstName;
            model.LastName = applicationUser.LastName;
            model.OrganizationId = applicationUser.OrganizationId;
           
            var lrole = context.UserRoles.Where(y => y.UserId == id);
            string roleName = "";
            if (lrole.Count() != 0)
            {
                string roleid = lrole.FirstOrDefault().RoleId;
                if (roleid != "")
                {
                    ApplicationRole role = context.ApplicationRole.Find(roleid);
                    roleName = role.Name;
                }
            }
            model.ApplicationRoleId = roleName;
           
            model.AvailableOrganizations = context.Organizations.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            model.AvailableRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();


            return View(model);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserListViewModel applicationUser)
        {
            applicationUser.AvailableOrganizations = context.Organizations.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            applicationUser.AvailableRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(applicationUser.Id);
                if (user != null)
                {
                    user.FirstName = applicationUser.FirstName;
                    user.LastName = applicationUser.LastName;
                    user.OrganizationId = applicationUser.OrganizationId;
                    var lRole = userManager.GetRolesAsync(user).Result;
                    string existingRole = lRole.Count() == 0 ? "":lRole.Single();
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (existingRole != applicationUser.ApplicationRoleId)
                        {
                            if(existingRole != "")
                                await userManager.RemoveFromRoleAsync(user, existingRole);
                            await userManager.AddToRoleAsync(user, applicationUser.ApplicationRoleId);
                        }
                            return RedirectToAction("Index");
                    }
                }
            }
            return View(applicationUser);

        }
    }
}




