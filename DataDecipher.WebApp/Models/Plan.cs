using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public class Plan
    {
        public string Id { get; set; }

        [Display(Name = "Plan name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }
        

    }
}
