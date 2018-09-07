using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDecipher.WebApp.Models;

namespace DataDecipher.WebApp.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public string Plan { get; set; }

        public virtual Method Methods { get; set; }

    }
}
