using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataDecipher.WebApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataDecipher.WebApp.Controllers
{
    public class RawDataController : Controller
    {
        // GET: /<controller>/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DisplayRawData(string inputSelectedFile)
        {
            var model1 = new RawData
            {
                //filePath = "TestData/GC2.DAT"
                FileName = inputSelectedFile,
                FilePath = "TestData/" + inputSelectedFile
            };
            model1.Data = model1.GetRawData(model1.FilePath);
            ViewResult result = View(model1);
            return View(model1);
        }
    }
}
