using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public class CsvParserConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        [Required(ErrorMessage = "Please provide a name")]
        [Display(Name = "Parser Name")]
        public string Name { get; set; }

        [Display(Name = "Parser Details")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Please provide a delimiter")]
        [Display(Name = "Delimiter")]
        public char Delimiter { get; set; }

        [Required(ErrorMessage = "Please provide one or more header column name(s)")]
        [Display(Name = "Header Column Names")]
        public string RequiredHeader { get; set; }

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

        public ICollection<SharedMethod> SharedUsers { get; set; }

        public ICollection<MethodDataSource> LinkedDataSources { get; set; }

        [NotMapped]
        public List<ApplicationUser> AvailableUsers { get; set; }
    }
}
