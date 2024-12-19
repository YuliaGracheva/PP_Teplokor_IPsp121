using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PP_Teplokor_IPsp121.View.New;
using System.Windows;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class ApplicationDiagnosticsViewModel : INotifyPropertyChanged
    {
        private CurrentUser currentUser;

        public ObservableCollection<ApplicationDiagnostics> ListApplicationDiagnostics { get; set; }
        public static int ClientID;

        public int MaxId()
        {
            int max = 0;
            if (this.ListApplicationDiagnostics != null)
            {
                foreach (var cl in this.ListApplicationDiagnostics)
                {
                    if (max < cl.ApplicationDiagnosticsID) max = cl.ApplicationDiagnosticsID;
                }
            }
            return max;
        }
        public ApplicationDiagnosticsViewModel(CurrentUser currentUsers)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<ApplicationDiagnostics> clients = MyDbContext.GetEntities<ApplicationDiagnostics>(connectionString, "SELECT * FROM ApplicationDiagnostics cl LEFT JOIN Archive a ON a.ApplicationDiagnosticsID = cl.ApplicationDiagnosticsID WHERE a.ArchiveID IS NULL;");
            currentUser = currentUsers;
            // Преобразование в ObservableCollection
            ListApplicationDiagnostics = new ObservableCollection<ApplicationDiagnostics>(clients);

        }
      
        private RelayCommand editApplicationDiagnostics;
        public RelayCommand EditApplicationDiagnostics
        {
            get
            {
                return editApplicationDiagnostics ??
                (editApplicationDiagnostics = new RelayCommand(obj =>
                {
                    WindowNewApplicationDiagnostics wnClient = new WindowNewApplicationDiagnostics()
                    { Title = "Редактирование сотрудника" };

                    ApplicationDiagnostics client = SelectedApplicationDiagnostics;
                    ApplicationDiagnostics tempClient = new ApplicationDiagnostics();

                    tempClient = client.ShallowCopy();
                    wnClient.DataContext = tempClient;
                    if (wnClient.ShowDialog() == true)
                    {
                        client.ApplicationDiagnosticsComment = tempClient.ApplicationDiagnosticsComment;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<ApplicationDiagnostics>(tempClient);
                    }
                }, (obj) => SelectedApplicationDiagnostics != null && ListApplicationDiagnostics.Count > 0));
            }
        }


        private ApplicationDiagnostics selectedApplicationDiagnostics;
        public ApplicationDiagnostics SelectedApplicationDiagnostics
        {
            get
            {
                return selectedApplicationDiagnostics;
            }
            set
            {
                selectedApplicationDiagnostics = value;
                OnPropertyChanged("SelectedApplicationDiagnostics");
                EditApplicationDiagnostics.CanExecute(true);
            }
        }

        private RelayCommand addApplicationDiagnostics;
        public RelayCommand AddApplicationDiagnostics
        {
            get
            {
                return addApplicationDiagnostics ??
                 (addApplicationDiagnostics = new RelayCommand(obj =>
                 {
                     WindowNewApplicationDiagnostics wnClient = new WindowNewApplicationDiagnostics()
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdClient = MaxId() + 1;
                     ApplicationDiagnostics client = new ApplicationDiagnostics { ClientID = maxIdClient };
                     wnClient.DataContext = client;
                     if (wnClient.ShowDialog() == true)
                     {
                         ListApplicationDiagnostics.Add(client);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<ApplicationDiagnostics>(client);
                     }
                     SelectedApplicationDiagnostics = client;
                 }));
            }
        }

        private RelayCommand deleteApplicationDiagnostics;
        public RelayCommand DeleteApplicationDiagnostics
        {
            get
            {
                return deleteApplicationDiagnostics ??
                (deleteApplicationDiagnostics = new RelayCommand(obj =>
                {
                    ApplicationDiagnostics client = SelectedApplicationDiagnostics;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные заявки по следующему адресу: г. " + "? ", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            ApplicationDiagnosticsID = selectedApplicationDiagnostics.ApplicationDiagnosticsID
                        };
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            dbContext.Archive.Add(archiveEntry);
                            dbContext.SaveChanges();
                        }
                    }
                }, (obj) => SelectedApplicationDiagnostics != null && ListApplicationDiagnostics.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}