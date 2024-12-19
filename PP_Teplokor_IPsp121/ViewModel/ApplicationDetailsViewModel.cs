using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
using PP_Teplokor_IPsp121.View.New;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class ApplicationDetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ApplicationDetails> ListApplicationDetails { get; set; }
        public static int ClientID;

        public int MaxId()
        {
            int max = 0;
            if (this.ListApplicationDetails != null)
            {
                foreach (var cl in this.ListApplicationDetails)
                {
                    if (max < cl.ApplicationDiagnosticsID) max = cl.ApplicationDiagnosticsID;
                }
            }
            return max;
        }
        public ApplicationDetailsViewModel(CurrentUser currentUser)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<ApplicationDetails> clients = MyDbContext.GetEntities<ApplicationDetails>(connectionString, "SELECT * FROM ApplicationDetails cl LEFT JOIN Archive a ON a.ApplicationDetailsID = cl.ApplicationDetailsID WHERE a.ArchiveID IS NULL;");

            // Преобразование в ObservableCollection
            ListApplicationDetails = new ObservableCollection<ApplicationDetails>(clients);

        }

        private RelayCommand editApplicationDiagnostics;
        public RelayCommand EditApplicationDetails
        {
            get
            {
                return editApplicationDiagnostics ??
                (editApplicationDiagnostics = new RelayCommand(obj =>
                {
                    WindowNewApplicationDetails wnClient = new WindowNewApplicationDetails
                    { Title = "Редактирование сотрудника" };

                    ApplicationDetails client = SelectedApplicationDetails;
                    ApplicationDetails tempClient = new ApplicationDetails();

                    tempClient = client.ShallowCopy();
                    wnClient.DataContext = tempClient;
                    if (wnClient.ShowDialog() == true)
                    {
                        client.ApplicationDiagnosticsID = tempClient.ApplicationDiagnosticsID;
                        client.DetailsID = tempClient.DetailsID;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<ApplicationDetails>(tempClient);
                    }
                }, (obj) => SelectedApplicationDetails != null && ListApplicationDetails.Count > 0));
            }
        }


        private ApplicationDetails selectedApplicationDiagnostics;
        public ApplicationDetails SelectedApplicationDetails
        {
            get
            {
                return selectedApplicationDiagnostics;
            }
            set
            {
                selectedApplicationDiagnostics = value;
                OnPropertyChanged("SelectedApplicationDetails");
                EditApplicationDetails.CanExecute(true);
            }
        }

        private RelayCommand addApplicationDiagnostics;
        public RelayCommand AddApplicationDetails
        {
            get
            {
                return addApplicationDiagnostics ??
                 (addApplicationDiagnostics = new RelayCommand(obj =>
                 {
                     WindowNewApplicationDetails wnClient = new WindowNewApplicationDetails
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdClient = MaxId() + 1;
                     ApplicationDetails client = new ApplicationDetails { ApplicationDetailsID = maxIdClient };
                     wnClient.DataContext = client;
                     if (wnClient.ShowDialog() == true)
                     {
                         ListApplicationDetails.Add(client);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<ApplicationDetails>(client);
                     }
                     SelectedApplicationDetails = client;
                 }));
            }
        }

        private RelayCommand deleteApplicationDiagnostics;
        public RelayCommand DeleteApplicationDetails
        {
            get
            {
                return deleteApplicationDiagnostics ??
                (deleteApplicationDiagnostics = new RelayCommand(obj =>
                {
                    ApplicationDetails client = SelectedApplicationDetails;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные заявки по следующему адресу: г. " + "? ", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            ApplicationDetailsID = selectedApplicationDiagnostics.ApplicationDetailsID
                        };
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            dbContext.Archive.Add(archiveEntry);
                            dbContext.SaveChanges();
                        }
                    }
                }, (obj) => SelectedApplicationDetails != null && ListApplicationDetails.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}