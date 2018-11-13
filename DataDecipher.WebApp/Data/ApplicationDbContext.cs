using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataDecipher.WebApp.Models;
using DataDecipher.WebApp.Data;

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
        public DbSet<DataSourceConnector> DataSourceConnectors { get; set; }
        public DbSet<SampleDataSource> SampleDataSources { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<SharedMethod> SharedMethods { get; set; }
        public DbSet<MethodDataSource> MethodDataSources { get; set; }
        public DbSet<PlanDataConnector> PlanDataConnectors { get; set; }
        public DbSet<DataDecipher.WebApp.Data.ApplicationRole> ApplicationRole { get; set; }
        public DbSet<DataDecipher.WebApp.Data.ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlanDataConnector>().HasKey(t => new { t.PlanId, t.DataSourceConnectorId });
            base.OnModelCreating(builder);

        }

        public DbSet<DataDecipher.WebApp.Models.DataProcessingRule> DataProcessingRule { get; set; }

       
    }
}
