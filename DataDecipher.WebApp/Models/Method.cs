using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataDecipher.WebApp.Models
{
    public class Method
    {
        [Display(Name = "Method name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Method description")]
        public string Description { get; set; }

        [Display(Name = "Created by")]
        public string Created_By { get; set; }

        [Display(Name = "Last modified date")]
        public string Last_Modified_Date { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
}