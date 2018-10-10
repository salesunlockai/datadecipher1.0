using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Data;
using DataDecipher.WebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace DataDecipher.WebApp.Controllers
{
    public class MethodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;

        public MethodsController(ApplicationDbContext context, UserManager<ApplicationUser> user)
        {
            _context = context;
            _user = user;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _user.GetUserAsync(HttpContext.User);

        // GET: Methods
        public async Task<IActionResult> Index()
        {
           
           
            //var sharedMethodsIds = _context.SharedMethods.Where(x => x.UserId == GetCurrentUserAsync().Result.Id).Include(y => y.Method).Select(z=>z.Method);

            var sharedMethods= _context.SharedMethods.Where(x => x.UserId == GetCurrentUserAsync().Result.Id);

            string sharedMethodsIds = String.Join(',',sharedMethods.Select(x => x.MethodId).ToArray());

            var methodList = await _context.Methods.Include(y=> y.SharedUsers).Include(y=>y.CreatedBy).Where(x => x.CreatedBy.Id == GetCurrentUserAsync().Result.Id || sharedMethodsIds.Contains(x.Id)).ToListAsync();

            //methodList.AddRange(sharedMethods);                               


            return View(methodList);
        }

        // GET: Methods/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @method = await _context.Methods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@method == null)
            {
                return NotFound();
            }

            return View(@method);
        }

        // GET: Methods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Methods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Method @method)
        {
            if (ModelState.IsValid)
            {
                method.CreatedBy = GetCurrentUserAsync().Result;
                method.CreatedDate = System.DateTime.Now;
                method.LastModifiedDate = System.DateTime.Now;
                method.Status = "Draft";
                _context.Add(@method);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@method);
        }

        // GET: Methods/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @method = await _context.Methods.FindAsync(id);
            if (@method == null)
            {
                return NotFound();
            }
            return View(@method);
        }

        // POST: Methods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,CreatedDate,CreatedBy,Status,LastModifiedDate")] Method @method)
        {
            if (id != @method.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    method.LastModifiedDate = System.DateTime.Now;
                    _context.Update(@method);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MethodExists(@method.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@method);
        }

        // GET: Methods/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @method = await _context.Methods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@method == null)
            {
                return NotFound();
            }

            return View(@method);
        }

        // POST: Methods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @method = await _context.Methods.FindAsync(id);
            _context.Methods.Remove(@method);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Methods/Details/5
        public async Task<IActionResult> Share(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @method = await _context.Methods
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (@method == null)
            {
                return NotFound();
            }

            ApplicationUser loggedUser = GetCurrentUserAsync().Result;
            @method.AvailableUsers = _context.ApplicationUser.Where(x => x.Id != loggedUser.Id && x.OrganizationId == loggedUser.OrganizationId).ToList();

            List<SharedMethod> sharedMethodUsers = _context.SharedMethods.Where(x => (x.MethodId == id)).ToList();
            if(sharedMethodUsers.Count > 0)
            {
                foreach(var user in sharedMethodUsers)
                {
                    ApplicationUser auser = @method.AvailableUsers.Where(x => x.Id == user.UserId).First();
                    auser.IsSelected = true;
                    auser.CanEditMethod = user.CanEdit;

                }
            }


            return View(@method);
        }

        // POST: Methods/Share/5
        [HttpPost, ActionName("Share")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShareConfirmed(string id, Method @method)
        {
            if (id != @method.Id)
            {
                return NotFound();
            }

           
            List<SharedMethod> sharedMethodUsers = _context.SharedMethods.Where(x=> (x.MethodId == id)).ToList();
            _context.RemoveRange(sharedMethodUsers);

            List<ApplicationUser> sharedUsers = @method.AvailableUsers.Where(x => x.IsSelected).ToList();
            if(sharedUsers.Count > 0)
            {
                foreach( var usr in sharedUsers)
                {
                    SharedMethod shared = new SharedMethod { MethodId = id, UserId = usr.Id, CanEdit = usr.CanEditMethod };
                    await  _context.AddAsync(shared);
                }
               
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

           
        }

        private bool MethodExists(string id)
        {
            return _context.Methods.Any(e => e.Id == id);
        }
    }
}
