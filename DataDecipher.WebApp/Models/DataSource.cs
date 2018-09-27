using System;
using System.ComponentModel.DataAnnotations;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public enum DataSourceType
    {
        Xml,
        Text,
        CSV,
        DAT
    }

    public class DataSource
    {
        public string Id { get; set; }

        [Display(Name = "Datasource name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Datasource description")]
        public string Description { get; set; }

        [Display(Name = "Datasource type")]
        public DataSourceType Type { get; set; }

        [Display(Name = "Datasource URL")]
        public string Uri { get; set; }

        [Display(Name = "Created by")]
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; }

    }
    public class SampleDataSource
    {
        public string Id { get; set; }

        [Display(Name = "Datasource name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Datasource description")]
        public string Description { get; set; }

        [Display(Name = "Datasource type")]
        public DataSourceType Type { get; set; }

        [Display(Name = "Datasource URL")]
        public string Uri { get; set; }

        [Display(Name = "Created by")]
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; }

    }
}
