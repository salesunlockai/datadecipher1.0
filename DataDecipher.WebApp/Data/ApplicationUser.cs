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

        public virtual Organization Organization { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual ICollection<Method> CreatedMethods { get; set; }

        public virtual ICollection<SharedMethod> SharedMethods { get; set; }

    }
}
