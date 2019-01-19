using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class XmlParserConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        [Required(ErrorMessage = "Please provide a name")]
        [Display(Name = "Parser Name")]
        public string Name { get; set; }

        [Display(Name = "Parser Details")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Please provide the parent tag to be extracted")]
        [Display(Name = "Mark the parent tag")]
        public string ParentTag { get; set; }

        [Required(ErrorMessage = "Please provide the header column name(s)")]
        [Display(Name = "Header Column Name(s)")]
        public string HeaderFields { get; set; }

        [Required(ErrorMessage = "Please provide the table column name(s)")]
        [Display(Name = "Table Column Name(s)")]
        public string TableFields { get; set; }

        [Display(Name = "Created by")]
        public ApplicationUser CreatedBy { get; set; }

        [Required]
        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Last modified date")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        //public ICollection<SharedMethod> SharedUsers { get; set; }

        public ICollection<MethodDataSource> LinkedDataSources { get; set; }

        [NotMapped]
        public List<ApplicationUser> AvailableUsers { get; set; }
    }
}
