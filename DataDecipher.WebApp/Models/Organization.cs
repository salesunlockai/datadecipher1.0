using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public class Organization
    {
        public string Id { get; set; }

        [Display(Name = "Organization name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        public string SelectedPlanId { get; set; }

        [ForeignKey("SelectedPlanId")]
        public Plan SelectedPlan { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

    }
}
