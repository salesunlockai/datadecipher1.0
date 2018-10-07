using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace DataDecipher.WebApp.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RawData
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string rawData { get; set; }

        public string GetRawData(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path);
            return null;
        }
    }
}