using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataDecipher.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;



namespace DataDecipher.WebApp
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MethodDBContext>

    {

        public MethodDBContext CreateDbContext(string[] args)

        {

            IConfigurationRoot configuration = new ConfigurationBuilder()

                .SetBasePath(Directory.GetCurrentDirectory())

                .AddJsonFile("appsettings.json")

                .Build();



            var builder = new DbContextOptionsBuilder<MethodDBContext>();



            var connectionString = configuration.GetConnectionString("DefaultConnection");



            builder.UseSqlServer(connectionString);



            return new MethodDBContext(builder.Options);

        }

    }
}
