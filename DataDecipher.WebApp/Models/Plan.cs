using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataDecipher.WebApp.Models
{
    public class Plan
    {
        public string Id { get; set; }

        [Display(Name = "Plan name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Enable dataconnectors")]
        [Required(ErrorMessage = "Please select atleast one data connector")]
        public ICollection<DataSourceType> EnabledDataConnectors { get; set; }

        [Display(Name = "Trial period (in days)")]
        public int TrialPeriod { get; set; }

        [Display(Name = "Monthly charges in USD (per month per user)")]
        public int Price { get; set; }

        public ICollection<Organization> Organizations { get; set; }
    }
}
