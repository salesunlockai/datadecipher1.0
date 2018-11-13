using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataDecipher.WebApp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DataDecipher.WebApp.Models
{
    public class DataProcessingRule
    {
        public string Id { get; set; }

        [Display(Name = "Rule name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Rule description")]
        public string Description { get; set; }

        [Display(Name = "Match condition")]
        [Required(ErrorMessage = "Please provide a valid match condition")]
        public string MatchCondition { get; set; }

        [Display(Name = "Replace with")]
        public string ReplaceWith { get; set; }

        public ICollection<DataSourceProcessingRule> DataSourceProcessingRules { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
