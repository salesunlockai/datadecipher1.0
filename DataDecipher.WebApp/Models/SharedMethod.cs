using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public class SharedMethod
    {
        
        public string Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public string MethodId { get; set; }

        [ForeignKey("MethodId")]
        public virtual Method Method { get; set; }
          
        public bool CanEdit { get; set; }

    }
}
