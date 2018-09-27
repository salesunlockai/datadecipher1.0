using System;
namespace DataDecipher.WebApp.Models
{
    public class MethodDataSource
    {
        public string Id { get; set; }
        public virtual Method Method { get; set; }
        public DataSource Datafile { get; set; }
    }
}
