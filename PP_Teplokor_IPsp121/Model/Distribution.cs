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
    public class Distribution : INotifyPropertyChanged
    {
        private string _dateOnly;
        private string _timeOnly;
        [Key]
        public int DistributionID { get; set; }
        public string DistributionDate
        {
            get { return _dateOnly; }
            set
            {
                _dateOnly = value;
                OnPropertyChanged("DistributionDate");
            }
        }
        public Employee SelectedEmployee { get; set; }
        public string DistributionTime
        {
            get { return _timeOnly; }
            set
            {
                _timeOnly = value;
                OnPropertyChanged("DistributionTime");
            }
        }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        [ForeignKey("ApplicationDiagnostics")]
        public int ApplicationDiagnosticsID { get; set; }
        public Distribution() { }
        public Distribution(int distributionID, string distributionDate, string distributionTime, int employeeID, int applicationDiagnosticsID)
        {
            DistributionID = distributionID;
            DistributionDate = distributionDate;
            DistributionTime = distributionTime;
            EmployeeID = employeeID;
            ApplicationDiagnosticsID = applicationDiagnosticsID;
        }
        private CurrentUser currentUser;
        public Distribution CopyFromClientEmployeeDPO(DistributionDPO criminalPersonDPO)
        {
            EmployeeViewModel vmCriminalProffession = new EmployeeViewModel(currentUser);

            int groupId = 0;

            foreach (var cl in vmCriminalProffession.ListEmployee)
            {
                if (cl.EmployeeSurname == criminalPersonDPO.EmployeeSurname)
                {
                    groupId = cl.EmployeeID;
                    break;
                }
            }

            if (groupId != 0)
            {
                this.DistributionID = criminalPersonDPO.DistributionID;
                this.ApplicationDiagnosticsID = criminalPersonDPO.ApplicationDiagnosticsID;
                this.DistributionTime = criminalPersonDPO.DistributionTime;
                this.DistributionDate = criminalPersonDPO.DistributionDate;
                this.EmployeeID = groupId;
            }
            return this;
        }
        public Distribution ShallowCopy()
        {
            return (Distribution)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
