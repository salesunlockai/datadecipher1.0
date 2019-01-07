using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Data;
using DataDecipher.WebApp.Models;
using Microsoft.AspNetCore.Identity;
namespace DataDecipher.WebApp.Controllers
{
    public class CsvParserConfigController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;

        public CsvParserConfigController(ApplicationDbContext context, UserManager<ApplicationUser> user)
        {
            _context = context;
            _user = user;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _user.GetUserAsync(HttpContext.User);

        // GET: CsvParserConfig
        public async Task<IActionResult> Index()
        {
            //var sharedParser = _context.SharedMethods.Where(x => x.UserId == GetCurrentUserAsync().Result.Id);

            //string sharedParserIds = String.Join(',', sharedParser.Select(x => x.MethodId).ToArray());

            //var parserList = await _context.Methods.Include(y => y.SharedUsers).Include(y => y.CreatedBy).Where(x => x.CreatedBy.Id == GetCurrentUserAsync().Result.Id || sharedMethodsIds.Contains(x.Id)).ToListAsync();

            return View(await _context.CsvParserConfigs.ToListAsync());
        }

        // GET: CsvParserConfig/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var csvParserConfig = await _context.CsvParserConfigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (csvParserConfig == null)
            {
                return NotFound();
            }

            return View(csvParserConfig);
        }

        // GET: CsvParserConfig/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CsvParserConfig/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Details,Delimiter,RequiredHeader,CreatedBy=GetCurrentUserAsync().Result")] CsvParserConfig csvParserConfig)
        {
            csvParserConfig.CreatedBy = GetCurrentUserAsync().Result;
            csvParserConfig.CreatedDate = DateTime.Now;
            csvParserConfig.LastModifiedDate = DateTime.Now;
            csvParserConfig.Status = "Active";
            if (ModelState.IsValid)
            {
                _context.Add(csvParserConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(csvParserConfig);
        }

        // GET: CsvParserConfig/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var csvParserConfig = await _context.CsvParserConfigs.FindAsync(id);
            if (csvParserConfig == null)
            {
                return NotFound();
            }
            return View(csvParserConfig);
        }

        // POST: CsvParserConfig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Details,Delimiter,RequiredHeader,CreatedBy,CreatedDate,LastModifiedDate,Status")] CsvParserConfig csvParserConfig)
        {
            if (id != csvParserConfig.ID)
            {
                return NotFound();
            }

            csvParserConfig.LastModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(csvParserConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CsvParserConfigExists(csvParserConfig.ID))
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
            return View(csvParserConfig);
        }

        // GET: CsvParserConfig/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var csvParserConfig = await _context.CsvParserConfigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (csvParserConfig == null)
            {
                return NotFound();
            }

            return View(csvParserConfig);
        }

        // POST: CsvParserConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var csvParserConfig = await _context.CsvParserConfigs.FindAsync(id);
            _context.CsvParserConfigs.Remove(csvParserConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CsvParserConfigExists(string id)
        {
            return _context.CsvParserConfigs.Any(e => e.ID == id);
        }
    }
}
