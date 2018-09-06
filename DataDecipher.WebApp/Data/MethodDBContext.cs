using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataDecipher.WebApp.Data
{
    public class MethodDBContext : DbContext
    {
        IConfiguration configuration;
        public MethodDBContext(IConfiguration config) 
        {
            configuration = config;
        }
        public DbSet<Method> Methods { get; set; }
      
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
