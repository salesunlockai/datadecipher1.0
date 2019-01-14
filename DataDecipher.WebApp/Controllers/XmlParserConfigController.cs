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
    public class XmlParserConfigController : Controller
    {
        private readonly ApplicationDbContext _context;

        public XmlParserConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: XmlParserConfig
        public async Task<IActionResult> Index()
        {
            return View(await _context.XmlParserConfigs.ToListAsync());
        }

        // GET: XmlParserConfig/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmlParserConfig = await _context.XmlParserConfigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (xmlParserConfig == null)
            {
                return NotFound();
            }

            return View(xmlParserConfig);
        }

        // GET: XmlParserConfig/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: XmlParserConfig/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Details,ParentTag,HeaderFields,TableFields,CreatedDate,LastModifiedDate,Status")] XmlParserConfig xmlParserConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(xmlParserConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(xmlParserConfig);
        }

        // GET: XmlParserConfig/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmlParserConfig = await _context.XmlParserConfigs.FindAsync(id);
            if (xmlParserConfig == null)
            {
                return NotFound();
            }
            return View(xmlParserConfig);
        }

        // POST: XmlParserConfig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Details,ParentTag,HeaderFields,TableFields,CreatedDate,LastModifiedDate,Status")] XmlParserConfig xmlParserConfig)
        {
            if (id != xmlParserConfig.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xmlParserConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XmlParserConfigExists(xmlParserConfig.ID))
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
            return View(xmlParserConfig);
        }

        // GET: XmlParserConfig/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xmlParserConfig = await _context.XmlParserConfigs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (xmlParserConfig == null)
            {
                return NotFound();
            }

            return View(xmlParserConfig);
        }

        // POST: XmlParserConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var xmlParserConfig = await _context.XmlParserConfigs.FindAsync(id);
            _context.XmlParserConfigs.Remove(xmlParserConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XmlParserConfigExists(string id)
        {
            return _context.XmlParserConfigs.Any(e => e.ID == id);
        }
    }
}
