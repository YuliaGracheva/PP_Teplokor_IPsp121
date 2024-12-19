using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PP_Teplokor_IPsp121.ViewModel;

namespace PP_Teplokor_IPsp121.Model
{
    public class ApplicationDiagnostics : INotifyPropertyChanged
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
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        [ForeignKey("Address")]
        public int AddressID {  get; set; }
        [ForeignKey("CategoryError")]
        public int CategoryErrorID { get; set; }

        public ApplicationDiagnostics() { }
        public ApplicationDiagnostics(int applicationDiagnosticsID, string applicationDiagnosticsComment, decimal price)
        {
            ApplicationDiagnosticsID = applicationDiagnosticsID;
            ApplicationDiagnosticsComment = applicationDiagnosticsComment;
            ApplicationDiagnosticsPrice = price;
        }
        private CurrentUser currentUser;
        public ApplicationDiagnostics CopyFromClientEmployeeDPO(ApplicationDiagnosticsDPO criminalPersonDPO)
        {
            ClientViewModel vmCriminalGroup = new ClientViewModel(currentUser);
            EmployeeViewModel vmCriminalProffession = new EmployeeViewModel(currentUser);
            CategoryErrorViewModel vmStatus = new CategoryErrorViewModel(currentUser);

            int groupId = 0;
            int proffessionId = 0;
            int statusId = 0;

            foreach (var cl in vmCriminalGroup.ListAddress)
            {
                if (cl.AddressCity == criminalPersonDPO.AddressClient)
                {
                    groupId = cl.AddressID;
                    break;
                }
            }

            foreach (var em in vmCriminalGroup.ListClient)
            {
                if (em.ClientNumber == criminalPersonDPO.ClientSurname)
                {
                    proffessionId = em.ClientID;
                    break;
                }
            }

            foreach (var ser in vmStatus.ListCategoryError)
            {
                if (ser.CategoryErrorName == criminalPersonDPO.CategoryErrorName)
                {
                    statusId = ser.CategoryErrorID;
                    break;
                }
            }

            if ((groupId != 0) && (proffessionId != 0) && (statusId != 0))
            {
                this.ApplicationDiagnosticsID = criminalPersonDPO.ApplicationDiagnosticsID;
                this.ApplicationDiagnosticsPrice = criminalPersonDPO.ApplicationDiagnosticsPrice;
                this.ApplicationDiagnosticsComment = criminalPersonDPO.ApplicationDiagnosticsComment;
                this.ClientID = proffessionId;
                this.AddressID = AddressID;
                this.CategoryErrorID = statusId;
            }
            return this;
        }

        public ApplicationDiagnostics ShallowCopy()
        {
            return (ApplicationDiagnostics)this.MemberwiseClone();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}