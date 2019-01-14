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
    public class TxtParserConfigController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TxtParserConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TxtParserConfig
        public async Task<IActionResult> Index()
        {
            return View(await _context.TxtParserConfigs.ToListAsync());
        }

        // GET: TxtParserConfig/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var txtParserConfig = await _context.TxtParserConfigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (txtParserConfig == null)
            {
                return NotFound();
            }

            return View(txtParserConfig);
        }

        // GET: TxtParserConfig/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TxtParserConfig/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Details,RecordMarkerStart,RecordMarkerEnd,HeaderMarkerStart,HeaderMarkerEnd,TableMarkerStart,TableMarkerEnd,HeaderFields,TableFields,Delimiter,CreatedDate,LastModifiedDate,Status")] TxtParserConfig txtParserConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(txtParserConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(txtParserConfig);
        }

        // GET: TxtParserConfig/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var txtParserConfig = await _context.TxtParserConfigs.FindAsync(id);
            if (txtParserConfig == null)
            {
                return NotFound();
            }
            return View(txtParserConfig);
        }

        // POST: TxtParserConfig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Details,RecordMarkerStart,RecordMarkerEnd,HeaderMarkerStart,HeaderMarkerEnd,TableMarkerStart,TableMarkerEnd,HeaderFields,TableFields,Delimiter,CreatedDate,LastModifiedDate,Status")] TxtParserConfig txtParserConfig)
        {
            if (id != txtParserConfig.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(txtParserConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TxtParserConfigExists(txtParserConfig.ID))
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
            return View(txtParserConfig);
        }

        // GET: TxtParserConfig/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var txtParserConfig = await _context.TxtParserConfigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (txtParserConfig == null)
            {
                return NotFound();
            }

            return View(txtParserConfig);
        }

        // POST: TxtParserConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var txtParserConfig = await _context.TxtParserConfigs.FindAsync(id);
            _context.TxtParserConfigs.Remove(txtParserConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TxtParserConfigExists(string id)
        {
            return _context.TxtParserConfigs.Any(e => e.ID == id);
        }
    }
}
