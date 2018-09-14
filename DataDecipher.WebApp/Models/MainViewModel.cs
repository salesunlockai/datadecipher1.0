using System;
using System.Collections.Generic;

namespace DataDecipher.WebApp.Models
{
    public class MainViewModel
    {
        public Method MethodViewModel { get; set; }
        public IEnumerable<DataSource> DataSourceViewModel { get; set; }
        public string SelectedDataSource { get; set; }
        public string RawData { get; set; }
    }
}
