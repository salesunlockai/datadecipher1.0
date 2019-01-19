using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TxtParserConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        [Required(ErrorMessage = "Please provide a name")]
        [Display(Name = "Parser Name")]
        public string Name { get; set; }

        [Display(Name = "Parser Details")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Please provide first string of the record")]
        [Display(Name = "Mark the start string of the record")]
        public string RecordMarkerStart { get; set; }

        [Required(ErrorMessage = "Please provide last string of the record")]
        [Display(Name = "Mark the end string of the record")]
        public string RecordMarkerEnd { get; set; }

        [Required(ErrorMessage = "Please provide first string of the header section")]
        [Display(Name = "Mark the start string of the header")]
        public string HeaderMarkerStart { get; set; }

        [Required(ErrorMessage = "Please provide last string of the header section")]
        [Display(Name = "Mark the start string of the header")]
        public string HeaderMarkerEnd { get; set; }

        [Display(Name = "Mark the start string of the tabular data")]
        public string TableMarkerStart { get; set; }

        [Display(Name = "Mark the end string of the tabular data")]
        public string TableMarkerEnd { get; set; }

        [Display(Name = "Header Column Name(s)")]
        public string HeaderFields { get; set; }

        [Display(Name = "Table Column Name(s)")]
        public string TableFields { get; set; }

        [Display(Name = "Delimiter")]
        public char Delimiter { get; set; }

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
