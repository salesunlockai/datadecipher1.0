using System;
using System.Collections.Generic;
using System.Data;

namespace DataDecipher.WebApp.Models
{
    public class MainViewModel
    {
    
        public string SelectedMethodId { get; set; }

        public Method SelectedMethod { get; set; }

        public List<Method> AvailableMethods { get; set; }


        public string SelectedDataSourceName { get; set; }

        public DataSource SelectedDataSource { get; set; }

        public List<DataSource> AvailableDataSources { get; set; }

        public string SelectedSampleDataSourceName { get; set; }

        public SampleDataSource SelectedSampleDataSource { get; set; }

        public List<SampleDataSource> AvailableSampleDataSources { get; set; }

        public string RawData { get; set; }

        //Here are all the properties required for a Data Processing/Cleansing
        public DataProcessingRule SelectedDataProcessingRule { get; set; }

        public List<DataProcessingRule> AvailableDataProcessingRules { get; set; }

        public string ProcessedData { get; set; }

        //Here are all the properties required for a Parser View
        public string SelectedParserId { get; set; }

        public ParserCsvFile SelectedParser { get; set; }

        public List<ParserCsvFile> AvailableParsers { get; set; }

        //Holds the Parsed Data for the currently selected source data

        public ParsedData parsedData { get; set; }

    }
}
