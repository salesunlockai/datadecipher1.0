using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataDecipher.WebApp.Models;
using System.IO;
using RestSharp;

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

            return PartialView("_RawData", model1);
           
        }

        [HttpPost]
        public IActionResult DisplayParsedData(string inputSelectedFile)
        {
            inputSelectedFile = "TestData/Raw/Sample_CSV.json";
            var model1 = new ParsedData
            {
                filePath = inputSelectedFile,
                fileName = Path.GetFileName(inputSelectedFile)
            };

            string fileExtension = Path.GetExtension(inputSelectedFile);
            if (fileExtension == ".csv")
            {
                string parsingRules = "{\"funcInput\":{\"attribute_list\" : [\"Testfile\",\"SampleName\",\"ExtCal.Average\",\"iCapOES\"],\"delimiter\" : \";\"  }}";
                //REST Call
                var client = new RestClient("https://fileparserapp.appspot.com/FileParser/CSV");
                var request = new RestRequest();
                request.Method = RestSharp.Method.POST;
                request.JsonSerializer.ContentType = "multipart/form-data";
                request.Parameters.Clear();
                request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
                request.AddParameter("ParsingRules", parsingRules);
                IRestResponse response = client.Execute(request);
                model1.parsedData = response.Content; // raw content as string
                model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
            }
            else if (fileExtension == ".xml")
            {
                string parsingRules = "{\"funcInput\": {\"parentTag\": \"TestOrder\",\"headerFields\": [\"TestOrder:Id\",\"TestOrder:LastModificationActorId\",\"TestOrder:Specimen\",\"Detail:Value\"],\"tableFields\": [\"TestOrder:TestOrderDate\",\"TestOrder:Status\",\"TestData:Id\",\"TestResult:Id\",\"ProcessOrder:Id\"]}}";
                //REST Call
                var client = new RestClient("https://fileparserapp.appspot.com/FileParser/XML");
                var request = new RestRequest();
                request.Method = RestSharp.Method.POST;
                request.JsonSerializer.ContentType = "multipart/form-data";
                request.Parameters.Clear();
                request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
                request.AddParameter("ParsingRules", parsingRules);
                IRestResponse response = client.Execute(request);
                model1.parsedData = response.Content; // raw content as string
                model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
            }
            else if (fileExtension == ".txt" || fileExtension == ".dat") {
                string parsingRules = "{\"funcInput\":{\"attribute_list\" : [\"Testfile\",\"SampleName\",\"ExtCal.Average\",\"iCapOES\"],\"delimiter\" : \";\"  }}";
                //REST Call
                var client = new RestClient("https://fileparserapp.appspot.com/FileParser/TXT");
                var request = new RestRequest();
                request.Method = RestSharp.Method.POST;
                request.JsonSerializer.ContentType = "multipart/form-data";
                request.Parameters.Clear();
                request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
                request.AddParameter("ParsingRules", parsingRules);
                IRestResponse response = client.Execute(request);
                model1.parsedData = response.Content; // raw content as string
                model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
            }
            else {
                model1.parsedData = "File Format Not Supported :( \r\n No Data To Display";
                model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
            }

            return PartialView("_ParsedData", model1);
        }
    }
}
