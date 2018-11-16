using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

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
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last modified date")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public ICollection<SharedMethod> SharedUsers { get; set; }

        public ICollection<MethodDataSource> LinkedDataSources { get; set; }

        [NotMapped]
        public List<ApplicationUser> AvailableUsers { get; set; }

    }


}