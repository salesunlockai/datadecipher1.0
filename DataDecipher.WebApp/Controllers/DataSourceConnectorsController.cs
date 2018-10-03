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
    public class DataSourceConnectorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataSourceConnectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DataSourceConnectors
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataSourceConnectors.ToListAsync());
        }

        // GET: DataSourceConnectors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSourceConnector = await _context.DataSourceConnectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSourceConnector == null)
            {
                return NotFound();
            }

            return View(dataSourceConnector);
        }

        // GET: DataSourceConnectors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataSourceConnectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Extension")] DataSourceConnector dataSourceConnector)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataSourceConnector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataSourceConnector);
        }

        // GET: DataSourceConnectors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSourceConnector = await _context.DataSourceConnectors.FindAsync(id);
            if (dataSourceConnector == null)
            {
                return NotFound();
            }
            return View(dataSourceConnector);
        }

        // POST: DataSourceConnectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Extension")] DataSourceConnector dataSourceConnector)
        {
            if (id != dataSourceConnector.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataSourceConnector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSourceConnectorExists(dataSourceConnector.Id))
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
            return View(dataSourceConnector);
        }

        // GET: DataSourceConnectors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSourceConnector = await _context.DataSourceConnectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSourceConnector == null)
            {
                return NotFound();
            }

            return View(dataSourceConnector);
        }

        // POST: DataSourceConnectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dataSourceConnector = await _context.DataSourceConnectors.FindAsync(id);
            _context.DataSourceConnectors.Remove(dataSourceConnector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataSourceConnectorExists(string id)
        {
            return _context.DataSourceConnectors.Any(e => e.Id == id);
        }
    }
}
