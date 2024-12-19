using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Model
{
    public class EmployeeRole : INotifyPropertyChanged
    {
        private string _name;
        [Key]
        public int EmployeeRoleID {  get; set; }
        public string EmployeeRoleName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("EmployeeRoleName");
            }
        }
        public EmployeeRole() {  }
        public EmployeeRole(string name)
        {
            EmployeeRoleName = name;
        }
        public EmployeeRole ShallowCopy()
        {
            return (EmployeeRole)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
