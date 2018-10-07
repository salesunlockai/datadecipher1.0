using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

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

        //public string GetParsedData(string path)
        //{
        //    if (File.Exists(path))
        //        return File.ReadAllText(path);
        //    return null;
        //}

        public DataTable GetParsedData(string path){
            DataTable dataTable = new DataTable();

            if (File.Exists(path)){
                foreach (var record in File.ReadAllLines(path))
                {
                    string[] row = record.Split(',');
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Length; i++)
                    {
                        dataRow[0] = row[0];
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;

            //string[] DataRecords = null;
            //List<string> listRecords = new List<string>();

            //if (File.Exists(path)){
            //    foreach (var record in File.ReadAllLines(path)){
            //        listRecords.Add(record);
            //    }
            //}
            //DataRecords = listRecords.ToArray();
            //return DataRecords;
            }
    }
}