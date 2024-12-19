using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Model
{
    public class DistributionItems
    {
        public string DistributionDate { get; set; }
        public string DistributionTime { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientName { get; set; }
        public string DetailsName { get; set; }
        public int ApplicationDiagnosticsID { get; set; }
        public decimal ApplicationDiagnosticsPrice { get; set; }
        public string AddressCity { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHome { get; set; }
        public string AddressNumber { get; set; }
        public Distribution Distribution { get; set; }
        public ApplicationDiagnostics ApplicationDiagnostics { get; set; }
        public Client Client { get; set; }
        public Address Address { get; set; }
        public ApplicationDetails ApplicationDetails { get; set; }
        public Details Details { get; set; }
    }

}
