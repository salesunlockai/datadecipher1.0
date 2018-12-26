using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataDecipher.WebApp.Models
{
    public class ParserCsvFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public char Delimiter { get; set; }
        [Required]
        [Display(Name = "Header Column Names")]
        public string RequiredHeader { get; set; }
    }
}
