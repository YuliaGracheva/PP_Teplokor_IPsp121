using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class DistributionCardViewModel
    {
        public Distribution Distribution { get; set; }
        public ApplicationDiagnostics ApplicationDiagnostics { get; set; }
        public Client Client { get; set; }
        public Address Address { get; set; }
        public ApplicationDetails ApplicationDetails { get; set; }
        public Details Details { get; set; }

        public DistributionCardViewModel()
        {
        }
    }
}