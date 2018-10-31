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
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

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

        public DataTable GetParsedDataTable(string content)
        {
            DataTable dtCsv = new DataTable();

            string[] rows = content.Split('\n'); //split full file text into rows  
            for (int i = 0; i < rows.Count() - 1; i++)
            {
                string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
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
            return dtCsv; //JsonConvert.DeserializeObject<ResponseModelType>(response);
        }


            //public string GetParsedData(string path)
            //{
            //    if (File.Exists(path))
            //        return File.ReadAllText(path);
            //    return null;
            //}

            //public DataTable GetParsedData(string path){
            //    DataTable dtCsv = new DataTable();
            //    string Fulltext;
            //    using (StreamReader sr = new StreamReader(path))
            //    {
            //        while (!sr.EndOfStream)
            //        {
            //            Fulltext = sr.ReadToEnd().ToString(); //read full file text  
            //            string[] rows = Fulltext.Split('\n'); //split full file text into rows  
            //            for (int i = 0; i < rows.Count() - 1; i++)
            //            {
            //                string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
            //                {
            //                    if (i == 0)
            //                    {
            //                        for (int j = 0; j < rowValues.Count(); j++)
            //                        {
            //                            dtCsv.Columns.Add(rowValues[j]); //add headers  
            //                        }
            //                    }
            //                    else
            //                    {
            //                        DataRow dr = dtCsv.NewRow();
            //                        for (int k = 0; k < rowValues.Count(); k++)
            //                        {
            //                            dr[k] = rowValues[k].ToString();
            //                        }
            //                        dtCsv.Rows.Add(dr); //add other rows  
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    return dtCsv;
            //}
        }
}