using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using DataDecipher.WebApp.Models;
using DataDecipher.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using RestSharp;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataDecipher.WebApp.Controllers
{
    public class MainController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> user;
        private readonly IConfiguration _configuration;


        public MainController(ApplicationDbContext ctx, UserManager<ApplicationUser> usr, IConfiguration cfg)
        {
            _context = ctx;
            user = usr;
            _configuration = cfg;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => user.GetUserAsync(HttpContext.User);

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            MainViewModel main = new MainViewModel();

            var sharedMethods = _context.SharedMethods.Where(x => x.UserId == GetCurrentUserAsync().Result.Id);
            string sharedMethodsIds = String.Join(',', sharedMethods.Select(x => x.MethodId).ToArray());
            var methodList = await _context.Methods.Include(y => y.SharedUsers).Include(y => y.CreatedBy).Where(x => x.CreatedBy.Id == GetCurrentUserAsync().Result.Id || sharedMethodsIds.Contains(x.Id)).ToListAsync();
            main.AvailableMethods = methodList;

            return View(main);
        }

        [HttpPost]
        public async Task<ActionResult> SelectMethod(MainViewModel main)
        {
            main.SelectedMethod = _context.Methods.Include(y=>y.LinkedDataSources).Where(x => x.Id == main.SelectedMethod.Id).First();

            if (main.SelectedMethod.LinkedDataSources.Count == 0)
            {
                var plan = _context.Organizations.Where(y => y.Id == GetCurrentUserAsync().Result.OrganizationId).Select(z => z.SelectedPlanId);
                var connectors = _context.PlanDataConnectors.Where(y => y.PlanId == plan.First()).Select(z => z.DataSourceConnectorId);
                var connectorIds = String.Join(',', connectors.ToArray());
                ViewBag.AvailableDataConnectors = _context.DataSourceConnectors.Where(y => connectorIds.Contains(y.Id)).ToList();

                main.AvailableDataSources = await _context.DataSources.Include(x => x.CreatedBy).Include(y => y.Type).Where(z => connectorIds.Contains(z.TypeId) && z.CreatedById == GetCurrentUserAsync().Result.Id).ToListAsync();
                main.AvailableSampleDataSources = await _context.SampleDataSources.Include(x => x.CreatedBy).Include(y => y.Type).Where(z => connectorIds.Contains(z.TypeId) && z.CreatedById == GetCurrentUserAsync().Result.Id).ToListAsync();

                main.SelectedDataSource = new DataSource();
            }
            else
            {
                main.SelectedDataSource = _context.MethodDataSources.Include(y => y.Datafile).Where(x => x.Id == main.SelectedMethod.LinkedDataSources.First().Id).Select(z=>z.Datafile).First();
                main.SelectedDataSourceName = main.SelectedDataSource.Uri;
            }

            return PartialView("_SetDataSource", main);

        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMethod(Models.Method method)
        {
            if (ModelState.IsValid)
            {
                method.CreatedBy = GetCurrentUserAsync().Result;
                method.CreatedDate = System.DateTime.Now;
                method.LastModifiedDate = System.DateTime.Now;
                method.Status = "Draft";
                _context.Methods.Add(method);
                await _context.SaveChangesAsync();
            }

            MainViewModel main = new MainViewModel();
            main.SelectedMethod = method;

            var plan = _context.Organizations.Where(y => y.Id == GetCurrentUserAsync().Result.OrganizationId).Select(z => z.SelectedPlanId);
            var connectors = _context.PlanDataConnectors.Where(y => y.PlanId == plan.First()).Select(z => z.DataSourceConnectorId);
            var connectorIds = String.Join(',', connectors.ToArray());
            ViewBag.AvailableDataConnectors = _context.DataSourceConnectors.Where(y => connectorIds.Contains(y.Id)).ToList();

            main.AvailableDataSources = await _context.DataSources.Include(x => x.CreatedBy).Include(y => y.Type).Where(z => connectorIds.Contains(z.TypeId) && z.CreatedById == GetCurrentUserAsync().Result.Id).ToListAsync();
            main.AvailableSampleDataSources = await _context.SampleDataSources.Include(x => x.CreatedBy).Include(y => y.Type).Where(z => connectorIds.Contains(z.TypeId) && z.CreatedById == GetCurrentUserAsync().Result.Id).ToListAsync();

            main.SelectedDataSource = new DataSource();
            main.SelectedMethod.LinkedDataSources = new List<MethodDataSource>();

            return PartialView("_SetDataSource", main);
        }

        [HttpPost]
        public ActionResult ShowSelectedMethodDetails(MainViewModel main, string SelectedMethodName)
        {
            main.SelectedMethod = _context.Methods.Where(method => method.Name == SelectedMethodName).First();
            return PartialView("~/Views/Methods/_DisplaySelectedMethod.cshtml", main);
        }

        [HttpPost]
        public async Task<ActionResult> SelectDataSource(MainViewModel main)
        {

            string fileName =  main.SelectedDataSourceName.Split('/').Last(); 

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare cloudFileShare = fileClient.GetShareReference(GetCurrentUserAsync().Result.Id);

            await cloudFileShare.CreateIfNotExistsAsync();

            CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            CloudFile cloudFile = cloudFileDirectory.GetFileReference(fileName);

            MemoryStream stream = new MemoryStream();

            await cloudFile.DownloadToStreamAsync(stream);

            stream.Position = 0;

            StreamReader reader = new StreamReader(stream);

            main.RawData = reader.ReadToEnd();

            //Copied raw data to processed data in case user skips the pre-processing/cleansing step
            main.ProcessedData = main.RawData;

            return PartialView("_DisplayDataSource", main);

        }

        [HttpPost]
        public async Task<ActionResult> SelectSampleDataSource(MainViewModel main)
        {
            main.SelectedDataSourceName = main.SelectedSampleDataSourceName;

            string fileName =  main.SelectedDataSourceName.Split('/').Last(); 

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share we created previously.
            CloudFileShare cloudFileShare = fileClient.GetShareReference("samples");

            await cloudFileShare.CreateIfNotExistsAsync();

            CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

            CloudFile cloudFile = cloudFileDirectory.GetFileReference(fileName);

            MemoryStream stream = new MemoryStream();

            await cloudFile.DownloadToStreamAsync(stream);

            stream.Position = 0;

            StreamReader reader = new StreamReader(stream);

            main.RawData = reader.ReadToEnd();

            //Copied raw data to processed data in case user skips the pre-processing/cleansing step
            main.ProcessedData = main.RawData;

            return PartialView("_DisplayDataSource", main);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewDataSource(MainViewModel main, IFormFile DataFile)
        {
            DataSource dataSource = main.SelectedDataSource;
            dataSource.DataFile = DataFile;

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
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                main.RawData = reader.ReadToEnd();
                main.SelectedDataSourceName = dataSource.DataFile.FileName;
            }

            dataSource.Uri = cloudFile.Uri.ToString();
            dataSource.CreatedBy = GetCurrentUserAsync().Result;
            dataSource.CreatedDate = System.DateTime.Now;

            _context.Add(dataSource);

            await _context.SaveChangesAsync();

            main.SelectedDataSourceName = cloudFile.Uri.ToString();

            //Copied raw data to processed data in case user skips the pre-processing/cleansing step
            main.ProcessedData = main.RawData;
          
            return PartialView("~/Views/Main/_DisplayDataSource.cshtml", main);
        }

        [HttpPost]
        public async Task<IActionResult> LinkDataSourceToMethod(MainViewModel main)
        {
            Models.Method method = _context.Methods.Include(x => x.LinkedDataSources).Where(sim => sim.Id == main.SelectedMethod.Id).First();
            DataSource dataSource = _context.DataSources.Where(arg => arg.Uri == main.SelectedDataSourceName).First();

            if (_context.MethodDataSources.Where(x => (x.Method.Id == method.Id) && (x.Datafile.Id == dataSource.Id)).Count() == 0)
            {
                _context.MethodDataSources.Add(new MethodDataSource { Method = method, Datafile = dataSource });
                await _context.SaveChangesAsync();
            }

            main.SelectedDataProcessingRule = new DataProcessingRule();
            main.AvailableDataProcessingRules = _context.DataProcessingRule.ToList();

            return PartialView("~/Views/Main/_ApplyRules.cshtml", main);
        }

        /// <summary>
        /// The following three methods are used to implement the data pre-processing functionality
        /// </summary>
        /// <returns>The rules.</returns>
        /// <param name="main">Main.</param>
        [HttpPost]
        public ActionResult ApplyRules(MainViewModel main)
        {
            List<DataProcessingRule> rules= main.AvailableDataProcessingRules.Where(sim => sim.IsSelected == true).ToList();

            foreach(DataProcessingRule r in rules)
            {
               DataProcessingRule rule = _context.DataProcessingRule.Where(sim => sim.Id == r.Id).First(); 
               main.ProcessedData = main.ProcessedData.Replace(rule.MatchCondition, rule.ReplaceWith, StringComparison.CurrentCultureIgnoreCase);
            }

            return PartialView("~/Views/Main/_DisplayProcessedData.cshtml", main);
        }

        [HttpPost]
        public ActionResult CreateAndApplyRule(MainViewModel main)
        {
            main.ProcessedData = main.ProcessedData.Replace(main.SelectedDataProcessingRule.MatchCondition, main.SelectedDataProcessingRule.ReplaceWith, StringComparison.CurrentCultureIgnoreCase);
            _context.DataProcessingRule.Add(main.SelectedDataProcessingRule);
            _context.SaveChanges();
            return PartialView("~/Views/Main/_DisplayProcessedData.cshtml", main);
        }

        [HttpPost]
        public ActionResult ApplyRule(MainViewModel main)
        {
            main.ProcessedData = main.ProcessedData.Replace(main.SelectedDataProcessingRule.MatchCondition, main.SelectedDataProcessingRule.ReplaceWith, StringComparison.CurrentCultureIgnoreCase);

            return PartialView("~/Views/Main/_DisplayProcessedData.cshtml", main);
        }

        [HttpPost]
        public ActionResult LoadSelectedProcessingRule(MainViewModel main, string SelectedProcessingRule, string ProcessedDataInProcessingRule)
        {
            main.ProcessedData = ProcessedDataInProcessingRule;
            main.SelectedDataProcessingRule = _context.DataProcessingRule.Where(pr => pr.Name == SelectedProcessingRule).First();
            return PartialView("~/Views/DataProcessingRules/_SelectedProcessingRUle.cshtml", main);
        }


        //This Method is called first time to load the Parser Configuration views. 
        [HttpPost]
        public ActionResult DisplayParserConfiguration(MainViewModel main, string ProcessedDataInDisplayProcessedData, string SelectedDataSourceNameInDisplayProcessedData)
        {
            main.SelectedParser = new ParserCsvFile();
            main.AvailableParsers = _context.ParserCsvFiles.ToList();
            main.ProcessedData = ProcessedDataInDisplayProcessedData;

            return PartialView("~/Views/Main/_SetParser.cshtml", main);
        }

        [HttpPost]
        public ActionResult LoadSelectedCsvParser(string SelectedParser, MainViewModel main, string SelectDataSourceNameInSetParser, string ProcessedDataInSetParser)
        {
            main.ProcessedData = ProcessedDataInSetParser;
            main.SelectedDataSourceName = SelectDataSourceNameInSetParser;
            main.SelectedParser = _context.ParserCsvFiles.Where(parser => parser.Name == SelectedParser).First();
            return PartialView("~/Views/ParserCsvFile/_SelectedCsvParserConfig.cshtml", main);
        }


        ////This method is used to create a new parser in case user enters 
        [HttpPost]
        public async Task<IActionResult> RunNewCsvParser(MainViewModel main, ParserCsvFile parserCsvFile, string ProcessedDataInSelectCsvParserConfig, bool isCheckedSaveParser = false)
        {
            main.ProcessedData = ProcessedDataInSelectCsvParserConfig;
            main.SelectedParser.Delimiter = parserCsvFile.Delimiter;
            if (ModelState.IsValid && isCheckedSaveParser)
            {
                _context.ParserCsvFiles.Add(main.SelectedParser);
                await _context.SaveChangesAsync();
            }

            main.parsedData = DisplayParsedCsvFile(main.ProcessedData, main.SelectedParser.RequiredHeader, main.SelectedParser.Delimiter.ToString());
            return PartialView("~/Views/Main/_ShowParsedData.cshtml", main);
        }

        [HttpPost]
        public IActionResult DisplayParsingViewTxtDat(string filePath)
        {
            var model1 = new RawData
            {
                FileName = Path.GetFileName(filePath),
                FilePath = filePath
            };
            model1.RawDataContent = GetRawData(model1.FilePath);

            return PartialView("_ParsingRulesTxtDat", model1);
        }

        [HttpPost]
        public IActionResult DisplayParsingViewCsv(string filePath)
        {
            var model1 = new RawData
            {
                FileName = Path.GetFileName(filePath),
                FilePath = filePath
            };
            model1.RawDataContent = GetRawData(model1.FilePath);

            return PartialView("_ParsingRulesCsv", model1);
        }

        [HttpPost]
        public IActionResult DisplayParsingViewXml(string filePath)
        {
            var model1 = new RawData
            {
                FileName = Path.GetFileName(filePath),
                FilePath = filePath
            };
            model1.RawDataContent = GetRawData(model1.FilePath);

            return PartialView("_ParsingRulesXml", model1);
        }

        [HttpPost]
        public IActionResult DisplayRawData(string inputSelectedFile)
        {
            var model1 = new RawData
            {
                FileName = Path.GetFileName(inputSelectedFile),
                FilePath = inputSelectedFile
            };
            model1.RawDataContent = GetRawData(model1.FilePath);

            return PartialView("_RawData", model1);

        }

        //[HttpGet]
        //public IActionResult DisplayDataSource()
        //{
        //    DataSource dataSource = ViewBag.SelectedDataSource;
          
        //    var model1 = new RawData
        //    {
        //        FileName = "sample.csv",
        //        FilePath = @"\Users\deepmalagupta\Projects\DataDecipher1.0\DataDecipher.WebApp\TestData\Raw\"
        //    };
           
        //   /* CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

        //    // Create a CloudFileClient object for credentialed access to Azure Files.
        //    CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

        //    // Get a reference to the file share we created previously.
        //    CloudFileShare cloudFileShare = fileClient.GetShareReference(GetCurrentUserAsync().Result.Id);

        //    cloudFileShare.CreateIfNotExistsAsync();

        //    CloudFileDirectory cloudFileDirectory = cloudFileShare.GetRootDirectoryReference();

        //    CloudFile cloudFile = cloudFileDirectory.GetFileReference(model1.fileName);
        //    cloudFile.DownloadToFileAsync(@"/Users/deepmalagupta/sample.csv",FileMode.Create);*/

        //    model1.RawDataContent = GetRawData(@"/Users/deepmalagupta/sample.csv");

        //    return PartialView("_RawData", model1);

        //}

        [HttpPost]
        public ParsedData DisplayParsedCsvFile(string inputSelectedFileData, string columns, string delimiter)
        {
            string[] columnNames = columns.Split(',');
            if (columnNames.Length != 0)
            {
                var model1 = new ParsedData
                {
                    //filePath = @"TestData/Raw/Sample.csv",
                    //fileName = Path.GetFileName(inputSelectedFile)
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
                //string inputSelectedFile = "TestData/Raw/Sample.csv";
                //System.IO.File.WriteAllText(inputSelectedFile, inputSelectedFileData);
                //request.AddFile("ParsingFile", inputSelectedFile); // adds to POST or URL querystring based on Method
                request.AddParameter("ParsingFile", inputSelectedFileData); // adds to POST or URL querystring based on Method
                request.AddParameter("ParsingRules", parsingRules);
                IRestResponse response = client.Execute(request);

                //Extract the results from the JSON
                dynamic parserResponse = JValue.Parse(response.Content);
                string returnStatus = parserResponse.Return_Status;
                string parsingStatus = parserResponse.Parsing_Status;
                string recordCount = parserResponse.Record_count;
                string parsedDataFromJson = parserResponse.outputFile;

                model1.parsedData = parsedDataFromJson; // raw content as string
                model1.parsedDataTable = GetParsedDataTable(model1.parsedData, delimiter);
                return model1;
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

            ////REST Call
            //var client = new RestClient("https://fileparserapp-new.appspot.com/FileParser/TXT");
            //var request = new RestRequest();
            //request.Method = RestSharp.Method.POST;
            //request.JsonSerializer.ContentType = "multipart/form-data";
            //request.Parameters.Clear();
            //request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
            //request.AddParameter("ParsingRules", parsingRules);
            //IRestResponse response = client.Execute(request);
            //model1.parsedData = response.Content; // raw content as string
            //model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData, delimiter);
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

            ////REST Call
            //var client = new RestClient("https://fileparserapp-new.appspot.com/FileParser/XML");
            //var request = new RestRequest();
            //request.Method = RestSharp.Method.POST;
            //request.JsonSerializer.ContentType = "multipart/form-data";
            //request.Parameters.Clear();
            //request.AddFile("ParsingFile", model1.filePath); // adds to POST or URL querystring based on Method
            //request.AddParameter("ParsingRules", parsingRules);
            //IRestResponse response = client.Execute(request);
            //model1.parsedData = response.Content; // raw content as string
            //model1.parsedDataTable = model1.GetParsedDataTable(model1.parsedData, ",");
            return PartialView("_ParsedData", model1);
        }

        private string GetRawData(string path)
        {
            if (System.IO.File.Exists(path))
                return System.IO.File.ReadAllText(path);
            return null;
        }

        private DataTable GetParsedDataTable(string content, string delimiter)
        {
            DataTable dtCsv = new DataTable();

            string[] rows = content.Split('\n'); //split full file text into rows  
            for (int i = 0; i < rows.Count() - 1; i++)
            {
                if (!string.IsNullOrEmpty(rows[i]) || !string.IsNullOrWhiteSpace(rows[i])) 
                { 
                    string[] rowValues = rows[i].Split(delimiter); //split each row with comma to get individual values  
                    {
                        if (i == 0)
                        {
                            for (int j = 0; j < rowValues.Count(); j++)
                            {
                                dtCsv.Columns.Add(rowValues[j]); //add headers  
                            }
                        }
                        else
                        {
                            DataRow dr = dtCsv.NewRow();
                            for (int k = 0; k < dtCsv.Columns.Count; k++)
                            {
                                dr[k] = rowValues[k].ToString();
                            }
                            dtCsv.Rows.Add(dr); //add other rows  
                        }
                    }
                }
            }
            return dtCsv; //JsonConvert.DeserializeObject<ResponseModelType>(response);
        }
    }
}
