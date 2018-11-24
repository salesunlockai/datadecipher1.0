using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataDecipher.WebApp.Models;
using DataDecipher.WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DataDecipher.WebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> user;
       
        public HomeController(UserManager<ApplicationUser> usr)
        {
            user = usr;
       
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => user.GetUserAsync(HttpContext.User);
       
        public IActionResult Index()
        {
            ApplicationUser usr = GetCurrentUserAsync().Result;

            if(usr != null)
            {
                return RedirectToAction("Index", "Main");
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
