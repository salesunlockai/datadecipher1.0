using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataDecipher.WebApp.Models
{
    public class Method
    {
        public string Id { get; set; }

        [Display(Name = "Method name")]
        [Required(ErrorMessage = "Please provide a name")]
        public string Name { get; set; }

        [Display(Name = "Method description")]
        public string Description { get; set; }

        [Display(Name = "Created by")]
        [ForeignKey("ApplicationUser")]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last modified date")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }


}