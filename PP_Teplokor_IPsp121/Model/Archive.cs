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
    public class Archive : INotifyPropertyChanged
    {
        [Key]
        public int ArchiveID { get; set; }

        [ForeignKey("ApplicationDetails")]
        public int? ApplicationDetailsID { get; set; }

        [ForeignKey("ApplicationDiagnostics")]
        public int? ApplicationDiagnosticsID { get; set; }

        [ForeignKey("Address")]
        public int? AddressID { get; set; }

        [ForeignKey("Client")]
        public int? ClientID { get; set; }

        [ForeignKey("CategoryError")]
        public int? CategoryErrorID { get; set; }

        [ForeignKey("Details")]
        public int? DetailsID { get; set; }

        [ForeignKey("Distribution")]
        public int? DistributionID { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }

        [ForeignKey("EmployeeRole")]
        public int? EmployeeRoleID { get; set; }

        public Archive() { }

        public Archive(int archiveId, int? applicationDetailsId, int? applicationDiagnosticsId,int? addressId, int? clientId, int? categoryErrorId,int? detailsId, int? distributionId,int? employeeId, int? employeeRoleId)
        {
            ArchiveID = archiveId;
            ApplicationDetailsID = applicationDetailsId;
            ApplicationDiagnosticsID = applicationDiagnosticsId;
            AddressID = addressId;
            ClientID = clientId;
            CategoryErrorID = categoryErrorId;
            DetailsID = detailsId;
            DistributionID = distributionId;
            EmployeeID = employeeId;
            EmployeeRoleID = employeeRoleId;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
