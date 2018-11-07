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
        public IActionResult DisplayParsingViewTxtDat(string filePath)
        {
            var model1 = new RawData
            {
                fileName = Path.GetFileName(filePath),
                filePath = filePath
            };
            model1.rawData = model1.GetRawData(model1.filePath);

            return PartialView("_ParsingRulesTxtDat", model1);
        }

        [HttpPost]
        public IActionResult DisplayParsingViewCsv(string filePath)
        {
            var model1 = new RawData
            {
                fileName = Path.GetFileName(filePath),
                filePath = filePath
            };
            model1.rawData = model1.GetRawData(model1.filePath);

            return PartialView("_ParsingRulesCsv", model1);
        }

        [HttpPost]
        public IActionResult DisplayParsingViewXml(string filePath)
        {
            var model1 = new RawData
            {
                fileName = Path.GetFileName(filePath),
                filePath = filePath
            };
            model1.rawData = model1.GetRawData(model1.filePath);

            return PartialView("_ParsingRulesXml", model1);
        }

        [HttpPost]
        public IActionResult DisplayRawData(string inputSelectedFile)
        {
            var model1 = new RawData
            {
                fileName = Path.GetFileName(inputSelectedFile),
                filePath = inputSelectedFile
            };
            model1.rawData = model1.GetRawData(model1.filePath);

            return PartialView("_RawData", model1);

        }

        [HttpPost]
        public IActionResult DisplayParsedCsvFile(string inputSelectedFile, string columns, string delimiter)
        {
            string[] columnNames = columns.Split(',');
            if (columnNames.Length != 0)
            {
                var model1 = new ParsedData
                {
                    filePath = inputSelectedFile,
                    fileName = Path.GetFileName(inputSelectedFile)
                };

                //string parsingRules = "{\"funcInput\":{\"attribute_list\" : [\"Testfile\",\"SampleName\",\"ExtCal.Average\",\"iCapOES\"],\"delimiter\" : \";\"  }}";
                string parsingRules = "{\"funcInput\":{\"attribute_list\":[\"";
                for (int i = 0; i < columnNames.Length; i++)
                {
                    if(i < (columnNames.Length - 1))
                        parsingRules = parsingRules + columnNames[i] + "\",\"";
                    else parsingRules = parsingRules + columnNames[i] + "\"],\"";
                }
                parsingRules = parsingRules + "delimiter\":\"" + delimiter + "\"}}";


                //REST Call
                var client = new RestClient("https://fileparserapp-new.appspot.com/FileParser/CSV");
                var request = new RestRequest();
                request.Method = RestSharp.Method.POST;
                request.JsonSerializer.ContentType = "multipart/form-data";
                request.Parameters.Clear();
                request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
                request.AddParameter("ParsingRules", parsingRules);
                IRestResponse response = client.Execute(request);
                model1.parsedData = response.Content; // raw content as string
                model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
                return PartialView("_ParsedData", model1);
            }
            else return null;
        }

        [HttpPost]
        public IActionResult DisplayParsedTxtDatFile(string inputSelectedFile, 
                                                     string recordMarkerStartText,
                                                     string recordMarkerEndText,
                                                     string tableMarkerStartText,
                                                     string tableMarkerEndText,
                                                     string headerMarkerStartText,
                                                     string headerMarkerEndText,
                                                     string headerFieldsSelection,
                                                     string tableFieldsSelection,
                                                     string delimiter)
        {
            string[] headerFields = headerFieldsSelection.Split(',');
            string[] tableFields = tableFieldsSelection.Split(',');
            var model1 = new ParsedData
            {
                filePath = inputSelectedFile,
                fileName = Path.GetFileName(inputSelectedFile)
            };

            //string parsingRules = "{\"funcInput\":{\"recordMarkerStartText\" : \"Software Version\",\"recordMarkerEndText\" : \"XXX Laboratories\",\"tableMarkerStartText\" : \"Water Report\",\"tableMarkerEndText\" : \"Timed Event Table\",\"headerMarkerStartText\" : \"Software Version\",\"headerMarkerEndText\" : \"Water Report\",\"headerFieldsSelection\" : [\"Sample Name\",\"Sample Number\",\"Instrument\",\"Sample Amount\"],\"tableFieldsSelection\" : [\"Time\",\"Component\",\"Adj Amt\",\"Area\"],\"delimiter\" : \",\"}}";
            string parsingRules = "{\"funcInput\":{\"recordMarkerStartText\":\"" + recordMarkerStartText
                                    + "\",\"recordMarkerEndText\":\"" + recordMarkerEndText
                                    + "\",\"tableMarkerStartText\":\"" + tableMarkerStartText
                                    + "\",\"tableMarkerEndText\":\"" + tableMarkerEndText
                                    + "\",\"headerMarkerStartText\":\"" + headerMarkerStartText
                + "\",\"headerMarkerEndText\" : \"" + headerMarkerEndText + "\",\"headerFieldsSelection\":[\"";
            for (int i = 0; i < headerFields.Length; i++)
            {
                if (i < (headerFields.Length - 1))
                    parsingRules = parsingRules + headerFields[i] + "\",\"";
                else parsingRules = parsingRules + headerFields[i] + "\"],\"tableFieldsSelection\":[\"";
            }
            for (int i = 0; i < tableFields.Length; i++)
            {
                if (i < (tableFields.Length - 1))
                    parsingRules = parsingRules + tableFields[i] + "\",\"";
                else parsingRules = parsingRules + tableFields[i] + "\"],\"";
            }
            parsingRules = parsingRules + "delimiter\":\"" + delimiter + "\"}}";

            //REST Call
            var client = new RestClient("https://fileparserapp-new.appspot.com/FileParser/TXT");
            var request = new RestRequest();
            request.Method = RestSharp.Method.POST;
            request.JsonSerializer.ContentType = "multipart/form-data";
            request.Parameters.Clear();
            request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
            request.AddParameter("ParsingRules", parsingRules);
            IRestResponse response = client.Execute(request);
            model1.parsedData = response.Content; // raw content as string
            model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
            return PartialView("_ParsedData", model1);
        }

        [HttpPost]
        public IActionResult DisplayParsedXmlFile(string inputSelectedFile, string parentTag, string headerFields, string tableFields)
        {
            string[] listHeaderFields = headerFields.Split(',');
            string[] listTableFields = tableFields.Split(',');
            var model1 = new ParsedData
            {
                filePath = inputSelectedFile,
                fileName = Path.GetFileName(inputSelectedFile)
            };

            //string parsingRules = "{\"funcInput\": {\"parentTag\": \"TestOrder\",\"headerFields\": [\"TestOrder:Id\",\"TestOrder:LastModificationActorId\",\"TestOrder:Specimen\"],\"tableFields\": [\"TestOrder:TestOrderDate\",\"TestOrder:Status\",\"TestData:Id\"]}}";
            string parsingRules = "{\"funcInput\":{\"parentTag\":\"" + parentTag + "\",\"headerFields\":[\"";
            for (int i = 0; i < listHeaderFields.Length; i++)
            {
                if (i < (listHeaderFields.Length - 1))
                    parsingRules = parsingRules + listHeaderFields[i] + "\",\"";
                else parsingRules = parsingRules + listHeaderFields[i] + "\"],\"tableFields\":[\"";
            }
            for (int i = 0; i < listTableFields.Length; i++)
            {
                if (i < (listTableFields.Length - 1))
                    parsingRules = parsingRules + listTableFields[i] + "\",\"";
                else parsingRules = parsingRules + listTableFields[i] + "\"]}}";
            }

            //REST Call
            var client = new RestClient("https://fileparserapp-new.appspot.com/FileParser/XML");
            var request = new RestRequest();
            request.Method = RestSharp.Method.POST;
            request.JsonSerializer.ContentType = "multipart/form-data";
            request.Parameters.Clear();
            request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
            request.AddParameter("ParsingRules", parsingRules);
            IRestResponse response = client.Execute(request);
            model1.parsedData = response.Content; // raw content as string
            model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData);
            return PartialView("_ParsedData", model1);
        }
    }
}
