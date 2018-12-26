using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Data;
using DataDecipher.WebApp.Models;

namespace DataDecipher.WebApp.Controllers
{
    public class ParserCsvFileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParserCsvFileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParserCsvFile
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParserCsvFiles.ToListAsync());
        }

        // GET: ParserCsvFile/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parserCsvFile = await _context.ParserCsvFiles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (parserCsvFile == null)
            {
                return NotFound();
            }

            return View(parserCsvFile);
        }

        // GET: ParserCsvFile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParserCsvFile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Details,Delimiter,RequiredHeader")] ParserCsvFile parserCsvFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parserCsvFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parserCsvFile);
        }

        // GET: ParserCsvFile/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parserCsvFile = await _context.ParserCsvFiles.FindAsync(id);
            if (parserCsvFile == null)
            {
                return NotFound();
            }
            return View(parserCsvFile);
        }

        // POST: ParserCsvFile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Details,Delimiter,RequiredHeader")] ParserCsvFile parserCsvFile)
        {
            if (id != parserCsvFile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parserCsvFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParserCsvFileExists(parserCsvFile.ID))
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
            return View(parserCsvFile);
        }

        // GET: ParserCsvFile/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parserCsvFile = await _context.ParserCsvFiles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (parserCsvFile == null)
            {
                return NotFound();
            }

            return View(parserCsvFile);
        }

        // POST: ParserCsvFile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var parserCsvFile = await _context.ParserCsvFiles.FindAsync(id);
            _context.ParserCsvFiles.Remove(parserCsvFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParserCsvFileExists(string id)
        {
            return _context.ParserCsvFiles.Any(e => e.ID == id);
        }
    }
}
