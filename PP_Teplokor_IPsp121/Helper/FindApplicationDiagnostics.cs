using PP_Teplokor_IPsp121.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Helper
{
        public class FindApplicationDiagnostics
    {
        int id;
            public FindApplicationDiagnostics(int id)
            {
                this.id = id;
            }
            public bool CriminalPersonPredicate(ApplicationDiagnostics criminalPerson)
            {
                return criminalPerson.ApplicationDiagnosticsID == id;
            }
        }
    }
