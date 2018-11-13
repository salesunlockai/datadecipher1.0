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
    public class DataProcessingRulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataProcessingRulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DataProcessingRules
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataProcessingRule.ToListAsync());
        }

        // GET: DataProcessingRules/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataProcessingRule = await _context.DataProcessingRule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataProcessingRule == null)
            {
                return NotFound();
            }

            return View(dataProcessingRule);
        }

        // GET: DataProcessingRules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataProcessingRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MatchCondition,ReplaceWith")] DataProcessingRule dataProcessingRule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataProcessingRule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataProcessingRule);
        }

        // GET: DataProcessingRules/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataProcessingRule = await _context.DataProcessingRule.FindAsync(id);
            if (dataProcessingRule == null)
            {
                return NotFound();
            }
            return View(dataProcessingRule);
        }

        // POST: DataProcessingRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,MatchCondition,ReplaceWith")] DataProcessingRule dataProcessingRule)
        {
            if (id != dataProcessingRule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataProcessingRule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataProcessingRuleExists(dataProcessingRule.Id))
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
            return View(dataProcessingRule);
        }

        // GET: DataProcessingRules/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataProcessingRule = await _context.DataProcessingRule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataProcessingRule == null)
            {
                return NotFound();
            }

            return View(dataProcessingRule);
        }

        // POST: DataProcessingRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dataProcessingRule = await _context.DataProcessingRule.FindAsync(id);
            _context.DataProcessingRule.Remove(dataProcessingRule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataProcessingRuleExists(string id)
        {
            return _context.DataProcessingRule.Any(e => e.Id == id);
        }
    }
}
