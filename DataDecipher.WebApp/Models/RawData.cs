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
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string RawDataContent { get; set; }
    }
}