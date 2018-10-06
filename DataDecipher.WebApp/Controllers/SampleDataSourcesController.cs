using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Data;
using DataDecipher.WebApp.Models;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.a

namespace DataDecipher.WebApp.Controllers
{
    public class SampleDataSourcesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;
        public SampleDataSourcesController(ApplicationDbContext ctx, UserManager<ApplicationUser> usr)
        {
            _context = ctx;
            _user = usr;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _user.GetUserAsync(HttpContext.User);

        // GET: SampleDataSources
        public async Task<IActionResult> Index()
        {
            return View(await _context.SampleDataSources.Include(x=> x.CreatedBy).ToListAsync());
        }

        // GET: SampleDataSources/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sampleDataSource = await _context.SampleDataSources.Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sampleDataSource == null)
            {
                return NotFound();
            }

            return View(sampleDataSource);
        }

        // GET: SampleDataSources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SampleDataSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Uri,CreatedDate,DataFile")] SampleDataSource sampleDataSource)
        {
            
            if (ModelState.IsValid)
            {
                if (sampleDataSource == null ||
                sampleDataSource.DataFile == null || sampleDataSource.DataFile.Length == 0)
                    return Content("file not selected");

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "SampleDataSets",
                    sampleDataSource.DataFile.FileName);

                sampleDataSource.Uri = path;
                sampleDataSource.CreatedBy = GetCurrentUserAsync().Result;
                sampleDataSource.CreatedDate = System.DateTime.Now;

                CloudStorageAccount storageAccount = null;
                CloudBlobContainer cloudBlobContainer = null;
                string sourceFile = null;
                string destinationFile = null;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await sampleDataSource.DataFile.CopyToAsync(stream);
                }
                _context.Add(sampleDataSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sampleDataSource);
        }

        // GET: SampleDataSources/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sampleDataSource = await _context.SampleDataSources.FindAsync(id);
            if (sampleDataSource == null)
            {
                return NotFound();
            }
            return View(sampleDataSource);
        }

        // POST: SampleDataSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,Uri,CreatedDate")] SampleDataSource sampleDataSource)
        {
            if (id != sampleDataSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sampleDataSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SampleDataSourceExists(sampleDataSource.Id))
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
            return View(sampleDataSource);
        }

        // GET: SampleDataSources/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sampleDataSource = await _context.SampleDataSources.Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sampleDataSource == null)
            {
                return NotFound();
            }

            return View(sampleDataSource);
        }

        // POST: SampleDataSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sampleDataSource = await _context.SampleDataSources.FindAsync(id);
            FileInfo file = new FileInfo(sampleDataSource.Uri);
            if (file.Exists)
                file.Delete();
            _context.SampleDataSources.Remove(sampleDataSource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SampleDataSourceExists(string id)
        {
            return _context.SampleDataSources.Any(e => e.Id == id);
        }
    }
}
