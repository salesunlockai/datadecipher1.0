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

        [Display(Name = "Select plan")]
        [Required(ErrorMessage = "Please select a plan")]
        public Plan SelectedPlan { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
