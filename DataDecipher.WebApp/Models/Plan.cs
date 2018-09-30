using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataDecipher.WebApp.Models
{
    public class Plan
    {
        public string Id { get; set; }

        [Display(Name = "Plan name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Enabled dataconnectors")]
        [Required(ErrorMessage = "Please select atleast one data connector")]
        public virtual ICollection<PlanDataConnector> EnabledDataConnectors { get; } = new List<PlanDataConnector>();

        [Display(Name = "Trial period (in days)")]
        [DefaultValue(0)]
        public int TrialPeriod { get; set; }

        [Display(Name = "Monthly charges in USD (per month per user)")]
        [DefaultValue(0)]
        public int Price { get; set; }

        public ICollection<Organization> Organizations { get; set; }

        [NotMapped]
        public List<DataSourceConnector> AvailableDataConnectors { get; set; }
    }
}
