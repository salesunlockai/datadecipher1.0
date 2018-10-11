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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Extensions.Configuration;

namespace DataDecipher.WebApp.Controllers
{
    public class SampleDataSourcesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly IConfiguration _configuration;

        public SampleDataSourcesController(ApplicationDbContext ctx, UserManager<ApplicationUser> usr, IConfiguration configuration)
        {
            _context = ctx;
            _user = usr;
            _configuration = configuration;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _user.GetUserAsync(HttpContext.User);

        // GET: SampleDataSources
        public async Task<IActionResult> Index()
        {
            return View(await _context.SampleDataSources.Include(x=> x.CreatedBy).Include(y=>y.Type).ToListAsync());
        }

        // GET: SampleDataSources/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sampleDataSource = await _context.SampleDataSources.Include(x => x.CreatedBy).Include(y => y.Type)
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
            ViewBag.AvailableDataConnectors = _context.DataSourceConnectors.ToList();
            return View();
        }

        // POST: SampleDataSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Uri,TypeId,CreatedDate,DataFile")] SampleDataSource sampleDataSource)
        {
            
            if (ModelState.IsValid)
            {
                if (sampleDataSource == null ||
                sampleDataSource.DataFile == null || sampleDataSource.DataFile.Length == 0)
                    return Content("file not selected");

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

                // Create a CloudFileClient object for credentialed access to Azure Files.
                CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

                // Get a reference to the file share we created previously.
                CloudFileShare cloudFileShare = fileClient.GetShareReference("samples");
                await cloudFileShare.CreateIfNotExistsAsync();

                CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

                CloudFile cloudFile = cloudFileDirectory.GetFileReference(sampleDataSource.DataFile.FileName);
                await cloudFile.DeleteIfExistsAsync();
                using (var stream = new MemoryStream())
                {
                    await sampleDataSource.DataFile.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    await cloudFile.UploadFromStreamAsync(stream);
                }

                sampleDataSource.Uri = cloudFile.Uri.ToString();
                sampleDataSource.CreatedBy = GetCurrentUserAsync().Result;
                sampleDataSource.CreatedDate = System.DateTime.Now;

                _context.Add(sampleDataSource);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewBag.AvailableDataConnectors = _context.DataSourceConnectors.ToList();
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
            sampleDataSource.CreatedBy = await _context.Users.FindAsync(sampleDataSource.CreatedById);
            sampleDataSource.Type = await _context.DataSourceConnectors.FindAsync(sampleDataSource.TypeId);
            return View(sampleDataSource);
        }

        // POST: SampleDataSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description")] SampleDataSource sampleDataSource)
        {
            if (id != sampleDataSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var getSampleDataSource = await _context.SampleDataSources.FindAsync(id);
                    getSampleDataSource.Name = sampleDataSource.Name;
                    getSampleDataSource.Description = sampleDataSource.Description;

                    _context.Update(getSampleDataSource);
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

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare cloudFileShare = fileClient.GetShareReference("samples");
            await cloudFileShare.CreateIfNotExistsAsync();

            CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            string cloudFilename = sampleDataSource.Uri.Split("/").Last();

            CloudFile cloudFile = cloudFileDirectory.GetFileReference(cloudFilename);
            await cloudFile.DeleteIfExistsAsync();


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
