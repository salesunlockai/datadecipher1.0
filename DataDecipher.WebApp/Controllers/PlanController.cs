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
    public class PlanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Plan
        public async Task<IActionResult> Index()
        {

            return View(await _context.Plans.Include( i => i.EnabledDataConnectors).ThenInclude((i => i.DataSourceConnector)).Include(e => e.Organizations).ToListAsync());
        }

        // GET: Plan/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Plan plan = await _context.Plans.Include(i => i.EnabledDataConnectors).ThenInclude((i => i.DataSourceConnector)).Include(e => e.Organizations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plan/Create
        public IActionResult Create()
        {
            
            Plan pass = new Plan();
            pass.AvailableDataConnectors = _context.DataSourceConnectors.ToList();
            return View(pass);
        }

        // POST: Plan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                foreach (DataSourceConnector i in plan.AvailableDataConnectors.Where(t=>t.IsSelected==true))
                {
                    _context.Add(new PlanDataConnector() { PlanId = plan.Id, DataSourceConnectorId = i.Id });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET: Plan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            List<PlanDataConnector> linkedDS = _context.PlanDataConnectors.Where(x=> x.PlanId == id ).ToList();
            if(linkedDS != null)
            {
                plan.AvailableDataConnectors = _context.DataSourceConnectors.Select(dc => new DataSourceConnector()
                { Id = dc.Id, Name = dc.Name, Extension = dc.Extension, IsSelected = linkedDS.Any(x=>x.DataSourceConnectorId == dc.Id)}).ToList();

            }
            return View(plan);
        }

        // POST: Plan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,TrialPeriod,Price,AvailableDataConnectors")] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    List<PlanDataConnector> linkedDS = _context.PlanDataConnectors.Where(x => x.PlanId == id).ToList();
                    if (linkedDS != null)
                    {
                        foreach(var item in linkedDS)
                            _context.Remove(item);

                    }
                    foreach (DataSourceConnector i in plan.AvailableDataConnectors.Where(t => t.IsSelected == true))
                    {
                        _context.Add(new PlanDataConnector() { PlanId = plan.Id, DataSourceConnectorId = i.Id });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            return View(plan);
        }

        // GET: Plan/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.Include(i => i.EnabledDataConnectors).ThenInclude((i => i.DataSourceConnector)).Include(e => e.Organizations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var plan = await _context.Plans.FindAsync(id);
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(string id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}
