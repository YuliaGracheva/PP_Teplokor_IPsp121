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
    public class ApplicationDiagnosticsDPO : INotifyPropertyChanged
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

        public ApplicationDiagnosticsDPO() { }
        public ApplicationDiagnosticsDPO(int applicationDiagnosticsID, string applicationDiagnosticsComment, decimal price, string surname, string sddress, string errorname)
        {
            ApplicationDiagnosticsID = applicationDiagnosticsID;
            ApplicationDiagnosticsComment = applicationDiagnosticsComment;
            ApplicationDiagnosticsPrice = price;
            ClientSurname = surname;
            AddressClient = sddress;
            CategoryErrorName = errorname;
        }
        private CurrentUser currentUser;
        public ApplicationDiagnosticsDPO CopyFromEmployee(ApplicationDiagnostics criminalPerson)
        {
            ApplicationDiagnosticsDPO criminalPersonDPO = new ApplicationDiagnosticsDPO();

            ClientViewModel vmCriminalGroup = new ClientViewModel(currentUser);
            EmployeeViewModel vmCriminalProffession = new EmployeeViewModel(currentUser);
            CategoryErrorViewModel vmStatus = new CategoryErrorViewModel(currentUser);

            string proffession = string.Empty;
            string groupName = string.Empty;
            string status = string.Empty;

            foreach (var r in vmCriminalGroup.ListClient)
            {
                if (r.ClientID == criminalPerson.ClientID)
                {
                    groupName = r.ClientNumber;
                    break;
                }
            }

            foreach (var r in vmCriminalGroup.ListAddress)
            {
                if (r.AddressID == criminalPerson.AddressID)
                {
                    proffession = r.AddressCity;
                    break;
                }
            }

            foreach (var r in vmStatus.ListCategoryError)
            {
                if (r.CategoryErrorID == criminalPerson.CategoryErrorID)
                {
                    status = r.CategoryErrorName;
                    break;
                }
            }

            if (proffession != string.Empty && groupName != string.Empty && status != string.Empty)
            {
                criminalPersonDPO.ApplicationDiagnosticsID = criminalPerson.ApplicationDiagnosticsID;
                criminalPersonDPO.ApplicationDiagnosticsPrice = criminalPerson.ApplicationDiagnosticsPrice;
                criminalPersonDPO.ApplicationDiagnosticsComment = criminalPerson.ApplicationDiagnosticsComment;
                criminalPersonDPO.AddressClient = proffession;
                criminalPersonDPO.CategoryErrorName = status;
                criminalPersonDPO.ClientSurname = groupName;
            }
            return criminalPersonDPO;
        }

        public ApplicationDiagnosticsDPO ShallowCopy()
        {
            return (ApplicationDiagnosticsDPO)this.MemberwiseClone();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}