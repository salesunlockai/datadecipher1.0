using System;
using System.Collections.Generic;

namespace DataDecipher.WebApp.Models
{
    public class MainViewModel
    {
        public string test { get; set; }

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


    }
}
