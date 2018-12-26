using System.Data;

namespace DataDecipher.WebApp.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ParsedData
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string parsedData { get; set; }
        public DataTable parsedDataTable { get; set; }
        public string[] records { get; set; }
    }
}