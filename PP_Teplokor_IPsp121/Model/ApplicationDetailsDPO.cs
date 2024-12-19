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
    public class ApplicationDetailsDPO : INotifyPropertyChanged
    {
        [Key]
        public int ApplicationDetailsID { get; set; }
        [ForeignKey("ApplicationDiagnostics")]
        public int ApplicationDiagnosticsID { get; set; }
        [ForeignKey("Details")]
        public string DetailsName { get; set; }
        public ApplicationDetailsDPO() { }
        public ApplicationDetailsDPO(int applicationDetailsID, int applicationDiagnosticsID, string detailsID)
        {
            ApplicationDetailsID = applicationDetailsID;
            ApplicationDiagnosticsID = applicationDiagnosticsID;
            DetailsName = detailsID;
        }
        private CurrentUser currentUser;
        public ApplicationDetailsDPO CopyFromEmployee(ApplicationDetails criminalPerson)
        {
            ApplicationDetailsDPO criminalPersonDPO = new ApplicationDetailsDPO();

            DetailsViewModel vmCriminalGroup = new DetailsViewModel(currentUser);

            string proffession = string.Empty;

            foreach (var r in vmCriminalGroup.ListDetails)
            {
                if (r.DetailsID == criminalPerson.DetailsID)
                {
                    proffession = r.DetailsName;
                    break;
                }
            }

            if (proffession != string.Empty)
            {
                criminalPersonDPO.ApplicationDiagnosticsID = criminalPerson.ApplicationDiagnosticsID;
                criminalPersonDPO.ApplicationDetailsID = criminalPerson.ApplicationDetailsID;
                criminalPersonDPO.DetailsName = proffession;
            }
            return criminalPersonDPO;
        }
        public ApplicationDetailsDPO ShallowCopy()
        {
            return (ApplicationDetailsDPO)this.MemberwiseClone();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
