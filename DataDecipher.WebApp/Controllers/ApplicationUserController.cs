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

        public ApplicationUserController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ApplicationUser> users = context.ApplicationUser.Include(x=>x.Organization).ToList();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (ApplicationUser x in users)
            {
                string roleid = context.UserRoles.Where(y => y.UserId == x.Id).First().RoleId;
                ApplicationRole role = context.ApplicationRole.Find(roleid);
                keyValuePairs.Add(x.Id, role.Name);
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
                Value = r.Id
            }).ToList();

            return View(model);
        }

        // POST: ApplicationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ApplicationUserListViewModel applicationUser)
        {
            applicationUser.AvailableOrganizations = context.Organizations.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            applicationUser.AvailableRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
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

            var applicationUser = await context.ApplicationUser.Include(x=>x.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            string roleid = context.UserRoles.Where(y => y.UserId == id).First().RoleId;
            ApplicationRole role = context.ApplicationRole.Find(roleid);

            ViewBag.LinkedRole = role.Name;
            return View(applicationUser);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            ApplicationUserListViewModel model = new ApplicationUserListViewModel();
            model.AvailableRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
            return PartialView("_AddUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(ApplicationUserListViewModel model)
        {
            Organization Ddorganization = new Organization { Name = "Data Decipher" };
            Plan FreePlan = new Plan { Name = "Free" };

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    UserName = model.UserName,
                    Email = model.Email,
                    LastName = "@datadecipher",
                    Organization = Ddorganization,
                    Plan = FreePlan
                };

                

                IdentityResult result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    ApplicationRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);
                    await userManager.AddPasswordAsync(user, model.Password);

                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel model = new EditUserViewModel();
            model.ApplicationRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    model.Name = user.FirstName;
                    model.Email = user.Email;
                    model.ApplicationRoleId = roleManager.Roles.Single(r => r.Name == userManager.GetRolesAsync(user).Result.Single()).Id;
                }
            }
            return PartialView("_EditUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FirstName = model.Name;
                    user.Email = model.Email;
                    string existingRole = userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = roleManager.Roles.Single(r => r.Name == existingRole).Id;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (existingRoleId != model.ApplicationRoleId)
                        {
                            IdentityResult roleResult = await userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                ApplicationRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return PartialView("_EditUser", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string name = string.Empty;
            ApplicationUserListViewModel deletedUserDetails = null;
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    deletedUserDetails = new ApplicationUserListViewModel();
                    deletedUserDetails.UserName = applicationUser.UserName;
                    deletedUserDetails.Email = applicationUser.Email;
                    //  deletedUserDetails.RoleName = applicationUser.Description;
                    //name = applicationUser.FirstName;
                }
            }
            return PartialView("_DeleteUser", deletedUserDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id, IFormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
    }
}
