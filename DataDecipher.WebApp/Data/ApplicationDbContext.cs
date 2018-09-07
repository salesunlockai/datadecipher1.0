using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Models;

namespace DataDecipher.WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<SharedMethod> SharedMethods { get; set; }
    }
}
