using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataDecipher.WebApp.Data;

namespace DataDecipher.WebApp.Models
{
    public class SharedMethod
    {
        
        public string Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Method Method { get; set; }
        public bool CanEdit { get; set; }

    }
}
