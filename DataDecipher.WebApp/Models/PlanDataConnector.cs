using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataDecipher.WebApp.Models
{
    public class PlanDataConnector
    {
        public String PlanId { get; set; }
        public Plan Plan { get; set; }
        public string DataSourceConnectorId { get; set; }
        public DataSourceConnector DataSourceConnector { get; set; }
    }
}
