using PP_Teplokor_IPsp121.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Helper
{
    public class FindEmployee
    {
        int id;
        public FindEmployee(int id)
        {
            this.id = id;
        }
        public bool CriminalPersonPredicate(Employee criminalPerson)
        {
            return criminalPerson.EmployeeID == id;
        }
    }
}
