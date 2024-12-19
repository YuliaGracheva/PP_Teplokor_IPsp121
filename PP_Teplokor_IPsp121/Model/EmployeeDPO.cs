using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PP_Teplokor_IPsp121.ViewModel;

namespace PP_Teplokor_IPsp121.Model
{
    public class EmployeeDPO : INotifyPropertyChanged
    {
        private string _surname;
        private string _name;
        private string? _patronymic;
        private string _login;
        private string _password;
        [Key]
        public int EmployeeID { get; set; }
        public string EmployeeSurname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("EmployeeSurname");
            }
        }
        public string EmployeeName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("EmployeeName");
            }
        }
        public string? EmployeePatronymic
        {
            get { return _patronymic; }
            set
            {
                _patronymic = value;
                OnPropertyChanged("EmployeePatronymic");
            }
        }
        public string EmployeeLogin
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("EmployeeLogin");
            }
        }
        public string EmployeePassword
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("EmployeePassword");
            }
        }
        private string _role;
        public string EmployeeRoleName
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged("EmployeeRoleName");
            }
        }
        public EmployeeDPO() { }
        public EmployeeDPO(int employeeID, string employeeSurname, string employeeName, string? employeePatronymic, string employeeLogin, string employeePassword, string role)
        {
            EmployeeID = employeeID;
            EmployeeSurname = employeeSurname;
            EmployeeName = employeeName;
            EmployeePatronymic = employeePatronymic;
            EmployeeLogin = employeeLogin;
            EmployeePassword = employeePassword;
            EmployeeRoleName = role;
        }
        private CurrentUser currentUser;
        public EmployeeDPO CopyFromEmployee(Employee criminalPerson)
        {
            EmployeeDPO criminalPersonDPO = new EmployeeDPO();

            EmployeeRoleViewModel vmCriminalProffession = new EmployeeRoleViewModel(currentUser);

            string proffession = string.Empty;

            foreach (var r in vmCriminalProffession.ListEmployeeRole)
            {
                if (r.EmployeeRoleID == criminalPerson.EmployeeRoleID)
                {
                    proffession = r.EmployeeRoleName;
                    break;
                }
            }

            if (proffession != string.Empty)
            {
                criminalPersonDPO.EmployeeName = criminalPerson.EmployeeName;
                criminalPersonDPO.EmployeeLogin = criminalPerson.EmployeeLogin;
                criminalPersonDPO.EmployeeSurname = criminalPerson.EmployeeSurname;
                criminalPersonDPO.EmployeeRoleName = proffession;
                criminalPersonDPO.EmployeePassword = criminalPerson.EmployeePassword;
                criminalPersonDPO.EmployeeID = criminalPerson.EmployeeID;
                criminalPersonDPO.EmployeePatronymic = criminalPerson.EmployeePatronymic;
            }
            return criminalPersonDPO;
        }

        public EmployeeDPO ShallowCopy()
        {
            return (EmployeeDPO)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}