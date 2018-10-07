using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataDecipher.WebApp.Models;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataDecipher.WebApp.Controllers
{
    public class MainController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            //List<DataSource> dataSources = new List<DataSource>();
            //dataSources.Add(new DataSource { Id="1", Name = "test", Description = "sample test data", Uri="TestData/test.txt", Type=DataSourceType.Text });
            //dataSources.Add(new DataSource { Id = "2", Name = "Gc2", Description = "sample test data", Uri = "TestData/GC2.dat", Type = DataSourceType.DAT });
            //dataSources.Add(new DataSource { Id = "3", Name = "LL_Spot_data", Description = "sample test data", Uri = "TestData/LL_SPOT_DATA.csv", Type = DataSourceType.CSV });
            //MainViewModel pass = new MainViewModel { DataSourceViewModel = dataSources };
            //return View(pass);
            return View();
        }

        [HttpPost]
        public IActionResult DisplayRawData(string inputSelectedFile)
        {
            var model1 = new RawData
            {
                //filePath = "TestData/GC2.DAT"
                fileName = inputSelectedFile,
                filePath = "TestData/Raw/" + inputSelectedFile
            };
            model1.rawData = model1.GetRawData(model1.filePath);

            return PartialView("_RawData",model1);
           
        }

        [HttpPost]
        public IActionResult DisplayParsedData(string inputSelectedFile)
        {
            var model1 = new ParsedData
            {
                filePath = "TestData/Parsed/GC2_output.csv",
                fileName = "GC2.DAT"
            };
            //model1.parsedData = model1.GetParsedData(model1.filePath);
            model1.parsedDataTable = model1.GetParsedData(model1.filePath);

            return View(model1);
        }


    }
}
