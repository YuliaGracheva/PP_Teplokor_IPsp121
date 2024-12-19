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
    public class DistributionDPO : INotifyPropertyChanged
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
        public string DistributionTime
        {
            get { return _timeOnly; }
            set
            {
                _timeOnly = value;
                OnPropertyChanged("DistributionTime");
            }
        }
        private string _employee;
        public string EmployeeSurname
        {
            get { return _employee; }
            set
            {
                _employee = value;
                OnPropertyChanged("EmployeeSurname");
            }
        }
        [ForeignKey("ApplicationDiagnostics")]
        public int ApplicationDiagnosticsID { get; set; }
        public DistributionDPO() { }
        public DistributionDPO(int distributionID, string distributionDate, string distributionTime,  int applicationDiagnosticsID, string surname)
        {
            DistributionID = distributionID;
            DistributionDate = distributionDate;
            DistributionTime = distributionTime;
            ApplicationDiagnosticsID = applicationDiagnosticsID;
            EmployeeSurname = surname;
        }

        private CurrentUser currentUser;
        public DistributionDPO CopyFromEmployee(Distribution criminalPerson)
        {
            DistributionDPO criminalPersonDPO = new DistributionDPO();

            EmployeeViewModel vmCriminalProffession = new EmployeeViewModel(currentUser);

            string proffession = string.Empty;

            foreach (var r in vmCriminalProffession.ListEmployee)
            {
                if (r.EmployeeID == criminalPerson.EmployeeID)
                {
                    proffession = r.EmployeeSurname;
                    break;
                }
            }

            if (proffession != string.Empty)
            {
                criminalPersonDPO.ApplicationDiagnosticsID = criminalPerson.ApplicationDiagnosticsID;
                criminalPersonDPO.DistributionDate = criminalPerson.DistributionDate;
                criminalPersonDPO.DistributionTime = criminalPerson.DistributionTime;
                criminalPersonDPO.EmployeeSurname = proffession;
                criminalPersonDPO.DistributionID = DistributionID;
            }
            return criminalPersonDPO;
        }

        public DistributionDPO ShallowCopy()
        {
            return (DistributionDPO)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
