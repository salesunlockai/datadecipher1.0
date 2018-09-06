using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace DataDecipher.WebApp.Data
{
    public class MethodDBContext : DbContext
    {

        public MethodDBContext(DbContextOptions<MethodDBContext> options) : base(options)
        {
        }
        public DbSet<Method> Methods { get; set; }
      
       
    }
}
