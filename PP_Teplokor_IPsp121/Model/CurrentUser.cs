using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Model
{
    public class CurrentUser
    {
        public int UserID { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsMaster { get; set; } = false;
        public bool IsEmployeeCall { get; set; } = false;
        public bool IsEmployeeDetails { get; set; } = false;
        public bool IsEmployeeEmployee { get; set; } = false;
        private List<EmployeeRole> employeeRoles;

        // Устанавливаем роль на основе информации о сотруднике
        public void SetRole(Employee employee)
        {
            UserID = employee.EmployeeID;

            if (employee.EmployeeRoleID != null)
            {
                switch (employee.EmployeeRoleID)
                {
                    case 1:
                        IsAdmin = true;
                        break;
                    case 2:
                        IsMaster = true;
                        break;
                    case 3:
                        IsEmployeeCall = true;
                        break;
                    case 4:
                        IsEmployeeDetails = true;
                        break;
                    case 5:
                        IsEmployeeEmployee = true;
                        break;
                    default:
                        ResetRoles();
                        break;
                }
            }
            else
            {
                ResetRoles();
            }
        }

        private void ResetRoles()
        {
            IsAdmin = false;
            IsMaster = false;
            IsEmployeeCall = false;
            IsEmployeeDetails = false;
            IsEmployeeEmployee = false;
        }

        // Реализуем Singleton
        private static CurrentUser _instance;
        private CurrentUser() { }

        public static CurrentUser Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CurrentUser();
                }
                return _instance;
            }
        }
    }

}