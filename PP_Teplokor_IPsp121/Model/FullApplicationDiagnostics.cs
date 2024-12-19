using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace PP_Teplokor_IPsp121.Model
{
    public class FullApplicationDiagnostics : INotifyPropertyChanged
    {
        private string _comment;
        private decimal _price;
        [Key]
        public int ApplicationDiagnosticsID { get; set; }
        public string ApplicationDiagnosticsComment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("ApplicationDiagnosticsComment");
            }
        }
        public decimal ApplicationDiagnosticsPrice
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("ApplicationDiagnosticsPrice");
            }
        }
        private string _surname;
        private string _address;
        private string _errorname;
        public string ClientSurname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("ClientSurname");
            }
        }
        public string AddressClient
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("AddressClient");
            }
        }
        public string CategoryErrorName
        {
            get { return _errorname; }
            set
            {
                _errorname = value;
                OnPropertyChanged("CategoryErrorName");
            }
        }
        private string _master;
        private string _date;
        private string _details;
        private string _time;
        public string MasterSurname
        {
            get { return _master; }
            set
            {
                _master = value;
                OnPropertyChanged("CategoryErrorName");
            }
        }
        public string DistributionDate
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("CategoryErrorName");
            }
        }
        public string DistributionTime
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("CategoryErrorName");
            }
        }
        public string DetailsList
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("CategoryErrorName");
            }
        }
        public FullApplicationDiagnostics() { }
        public FullApplicationDiagnostics(int applicationDiagnosticsID, string applicationDiagnosticsComment, decimal applicationDiagnosticsPrice,  string clientSurname, string addressClient, string categoryErrorName, string masterSurname, string distributionDate, string distributionTime, string detailsList)
        {
            ApplicationDiagnosticsID = applicationDiagnosticsID;
            ApplicationDiagnosticsComment = applicationDiagnosticsComment;
            ApplicationDiagnosticsPrice = applicationDiagnosticsPrice;
            ClientSurname = clientSurname;
            AddressClient = addressClient;
            CategoryErrorName = categoryErrorName;
            MasterSurname = masterSurname;
            DistributionDate = distributionDate;
            DistributionTime = distributionTime;
            DetailsList = detailsList;
        }

        public FullApplicationDiagnostics ShallowCopy()
        {
            return (FullApplicationDiagnostics)this.MemberwiseClone();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
