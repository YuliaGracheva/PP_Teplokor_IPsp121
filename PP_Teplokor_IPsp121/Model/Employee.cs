using PP_Teplokor_IPsp121.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Model
{
    public class Employee : INotifyPropertyChanged
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
        [ForeignKey("EmployeeRole")]
        public int EmployeeRoleID { get; set; }
        public Employee() { }
        public Employee(int employeeID, string employeeSurname, string employeeName, string? employeePatronymic, string employeeLogin, string employeePassword, int employeeRoleID)
        {
            EmployeeID = employeeID;
            EmployeeSurname = employeeSurname;
            EmployeeName = employeeName;
            EmployeePatronymic = employeePatronymic;
            EmployeeLogin = employeeLogin;
            EmployeePassword = employeePassword;
            EmployeeRoleID = employeeRoleID;
        }
        private CurrentUser currentUser;
        public Employee CopyFromClientEmployeeDPO(EmployeeDPO criminalPersonDPO)
        {
            EmployeeRoleViewModel vmCriminalProffession = new EmployeeRoleViewModel(currentUser);

            int groupId = 0;

            foreach (var cl in vmCriminalProffession.ListEmployeeRole)
            {
                if (cl.EmployeeRoleName == criminalPersonDPO.EmployeeRoleName)
                {
                    groupId = cl.EmployeeRoleID;
                    break;
                }
            }

            if (groupId != 0)
            {
                this.EmployeeName = criminalPersonDPO.EmployeeName;
                this.EmployeeLogin = criminalPersonDPO.EmployeeLogin;
                this.EmployeeSurname = criminalPersonDPO.EmployeeSurname;
                this.EmployeePassword = criminalPersonDPO.EmployeePassword;
                this.EmployeeID = criminalPersonDPO.EmployeeID;
                this.EmployeePatronymic = criminalPersonDPO.EmployeePatronymic;
                this.EmployeeRoleID = groupId;
            }
            return this;
        }
        public string FullName => $"{EmployeeSurname} {EmployeeName}";
        public Employee ShallowCopy()
        {
            return (Employee)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
