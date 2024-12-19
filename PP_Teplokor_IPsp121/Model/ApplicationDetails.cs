using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PP_Teplokor_IPsp121.ViewModel;

namespace PP_Teplokor_IPsp121.Model
{
    public class ApplicationDetails : INotifyPropertyChanged
    {
        [Key]
        public int ApplicationDetailsID { get; set; }
        [ForeignKey("ApplicationDiagnostics")]
        public int ApplicationDiagnosticsID { get; set; }
        [ForeignKey("Details")]
        public int DetailsID { get; set; }
        public ApplicationDetails() { }
        public ApplicationDetails(int applicationDetailsID, int applicationDiagnosticsID, int detailsID)
        {
            ApplicationDetailsID = applicationDetailsID;
            ApplicationDiagnosticsID = applicationDiagnosticsID;
            DetailsID = detailsID;
        }

        private CurrentUser currentUser;
        public ApplicationDetails CopyFromClientEmployeeDPO(ApplicationDetailsDPO criminalPersonDPO)
        {
            DetailsViewModel vmCriminalGroup = new DetailsViewModel(currentUser);
            int groupId = 0;

            foreach (var cl in vmCriminalGroup.ListDetails)
            {
                if (cl.DetailsName == criminalPersonDPO.DetailsName)
                {
                    groupId = cl.DetailsID;
                    break;
                }
            }

            if (groupId != 0)
            {
                this.ApplicationDiagnosticsID = criminalPersonDPO.ApplicationDiagnosticsID;
                this.ApplicationDetailsID = criminalPersonDPO.ApplicationDetailsID;
                this.DetailsID = groupId;
            }
            return this;
        }

        public ApplicationDetails ShallowCopy()
        {
            return (ApplicationDetails)this.MemberwiseClone();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
