using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataDecipher.WebApp.Models
{
    public class DataSourceProcessingRule
    {
        public string Id { get; set; }

        public string DataProcessingRuleId { get; set; }

        [ForeignKey("DataProcessingRuleId")]
        public virtual DataProcessingRule Rule { get; set; }

        public string DataSourceId { get; set; }

        [ForeignKey("DataSourceId")]
        public virtual DataSource DataSource { get; set; }

    }
}
