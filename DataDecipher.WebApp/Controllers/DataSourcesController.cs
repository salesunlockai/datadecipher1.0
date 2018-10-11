using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Data;
using DataDecipher.WebApp.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace DataDecipher.WebApp.Controllers
{
    public class DataSourcesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly IConfiguration _configuration;

        public DataSourcesController(ApplicationDbContext context, UserManager<ApplicationUser> usr, IConfiguration configuration)
        {
            _context = context;
            _user = usr;
            _configuration = configuration;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _user.GetUserAsync(HttpContext.User);
        // GET: DataSources
        public async Task<IActionResult> Index()
        {
            
            var plan = _context.Organizations.Where(y => y.Id == GetCurrentUserAsync().Result.OrganizationId).Select(z=>z.SelectedPlanId);
            var connectors = _context.PlanDataConnectors.Where(y => y.PlanId == plan.First()).Select(z=>z.DataSourceConnectorId);
            var connectorIds = String.Join(',',connectors.ToArray());
                    
            return View(await _context.DataSources.Include(x => x.CreatedBy).Include(y => y.Type).Where(z=> connectorIds.Contains(z.TypeId) && z.CreatedById == GetCurrentUserAsync().Result.Id).ToListAsync());
        }

        // GET: DataSources/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources.Include(x => x.CreatedBy).Include(y => y.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSource == null)
            {
                return NotFound();
            }

            return View(dataSource);
        }

        // GET: DataSources/Create
        public IActionResult Create()
        {
            var plan = _context.Organizations.Where(y => y.Id == GetCurrentUserAsync().Result.OrganizationId).Select(z => z.SelectedPlanId);
            var connectors = _context.PlanDataConnectors.Where(y => y.PlanId == plan.First()).Select(z => z.DataSourceConnectorId);
            var connectorIds = String.Join(',', connectors.ToArray());

            ViewBag.AvailableDataConnectors = _context.DataSourceConnectors.Where(y=>connectorIds.Contains(y.Id)).ToList();
            return View();
        }

        // POST: DataSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Uri,TypeId,CreatedDate,DataFile")] DataSource dataSource)
        {
            if (ModelState.IsValid)
            {
            if (dataSource == null ||
                dataSource.DataFile == null || dataSource.DataFile.Length == 0)
                return Content("file not selected");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare cloudFileShare = fileClient.GetShareReference(GetCurrentUserAsync().Result.Id);
            await cloudFileShare.CreateIfNotExistsAsync();

            CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            CloudFile cloudFile = cloudFileDirectory.GetFileReference(dataSource.DataFile.FileName);
            await cloudFile.DeleteIfExistsAsync();
            using (var stream = new MemoryStream())
            {
                await dataSource.DataFile.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await cloudFile.UploadFromStreamAsync(stream);
            }

            dataSource.Uri = cloudFile.Uri.ToString();
            dataSource.CreatedBy = GetCurrentUserAsync().Result;
            dataSource.CreatedDate = System.DateTime.Now;

            _context.Add(dataSource);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            }
            var plan = _context.Organizations.Where(y => y.Id == GetCurrentUserAsync().Result.OrganizationId).Select(z => z.SelectedPlanId);
            var connectors = _context.PlanDataConnectors.Where(y => y.PlanId == plan.First()).Select(z => z.DataSourceConnectorId);
            var connectorIds = String.Join(',', connectors.ToArray());

            ViewBag.AvailableDataConnectors = _context.DataSourceConnectors.Where(y => connectorIds.Contains(y.Id)).ToList();

            return View(dataSource);
        }

        // GET: DataSources/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources.FindAsync(id);
            if (dataSource == null)
            {
                return NotFound();
            }
            dataSource.CreatedBy = await _context.Users.FindAsync(dataSource.CreatedById);
            dataSource.Type = await _context.DataSourceConnectors.FindAsync(dataSource.TypeId);
            return View(dataSource);
        }

        // POST: DataSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description")] DataSource dataSource)
        {
            if (id != dataSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var getDataSource = await _context.DataSources.FindAsync(id);
                    getDataSource.Name = dataSource.Name;
                    getDataSource.Description = dataSource.Description;

                    _context.Update(getDataSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSourceExists(dataSource.Id))
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
            return View(dataSource);
        }

        // GET: DataSources/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSource = await _context.DataSources.Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataSource == null)
            {
                return NotFound();
            }

            return View(dataSource);
        }

        // POST: DataSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dataSource = await _context.DataSources.FindAsync(id);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare cloudFileShare = fileClient.GetShareReference(GetCurrentUserAsync().Result.Id);
            await cloudFileShare.CreateIfNotExistsAsync();

            CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            string cloudFilename = dataSource.Uri.Split("/").Last();

            CloudFile cloudFile = cloudFileDirectory.GetFileReference(cloudFilename);
            await cloudFile.DeleteIfExistsAsync();


            _context.DataSources.Remove(dataSource);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataSourceExists(string id)
        {
            return _context.DataSources.Any(e => e.Id == id);
        }
    }
}
