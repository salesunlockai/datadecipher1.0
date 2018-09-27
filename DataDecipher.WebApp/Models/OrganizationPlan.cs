using System;
namespace DataDecipher.WebApp.Models
{
    public class OrganizationPlan
    {
        public string Id { get; set; }
        public Organization Organization { get; set; }
        public Plan  Plan { get; set; }
    }
}
