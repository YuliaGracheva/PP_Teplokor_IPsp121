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
using PP_Teplokor_IPsp121.View.New;
using System.Windows;
using System.Windows.Controls;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Details> ListDetails { get; set; }
        public static int DetailsID;
        public int MaxId()
        {
            int max = 0;
            if (this.ListDetails != null)
            {
                foreach (var cl in this.ListDetails)
                {
                    if (max < cl.DetailsID) max = cl.DetailsID;
                }
            }
            return max;
        }
        public ObservableCollection<string> MenuItems { get; set; }
        public DetailsViewModel(CurrentUser currentUser)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<Details> clients = MyDbContext.GetEntities<Details>(connectionString, "SELECT * FROM Details WHERE DetailsID NOT IN (SELECT DetailsID FROM Archive)");

            // Преобразование в ObservableCollection
            ListDetails = new ObservableCollection<Details>(clients);
            if (currentUser.IsAdmin)
            {
                MenuItems = new ObservableCollection<string>
            {
                "На главную",
                "Заявки",
                "Сотрудники",
                "Детали",
                "Категории ошибок",
                "Распределение",
                "Выйти из аккаунта",
                "Выйти из приложения"
            };
            }
            if (currentUser.IsEmployeeEmployee)
            {
                MenuItems = new ObservableCollection<string>
            {
                "На главную",
                "Сотрудники",
                "Выйти из аккаунта",
                "Выйти из приложения"
            };
            }
            else if (currentUser.IsEmployeeDetails)
            {
                MenuItems = new ObservableCollection<string>
            {
                "На главную",
                "Детали",
                "Выйти из аккаунта",
                "Выйти из приложения"
            };
            }
            else if (currentUser.IsEmployeeCall)
            {
                MenuItems = new ObservableCollection<string>
            {
                "На главную",
                "Заявки",
                "Категории ошибок",
                "Выйти из аккаунта",
                "Выйти из приложения"
            };
            }
            else if (currentUser.IsMaster)
            {
                MenuItems = new ObservableCollection<string>
            {
                "На главную",
                "Распределение",
                "Выйти из аккаунта",
                "Выйти из приложения"
            };
            }
        }

        private RelayCommand editDetails;
        public RelayCommand EditDetails
        {
            get
            {
                return editDetails ??
                (editDetails = new RelayCommand(obj =>
                {
                    WindowNewDetails wnDetails = new WindowNewDetails
                    { Title = "Редактирование сотрудника" };

                    Details client = SelectedDetails;
                    Details tempClient = new Details();

                    tempClient = client.ShallowCopy();
                    wnDetails.DataContext = tempClient;
                    if (wnDetails.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        client.DetailsName = tempClient.DetailsName;
                        client.DetailsDescription = tempClient.DetailsDescription;
                        client.DetailsCount = tempClient.DetailsCount;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<Details>(tempClient);
                    }
                }, (obj) => SelectedDetails != null && ListDetails.Count > 0));
            }
        }


        private Details selectedDetails;
        public Details SelectedDetails
        {
            get
            {
                return selectedDetails;
            }
            set
            {
                selectedDetails = value;
                OnPropertyChanged("SelectedDetails");
                EditDetails.CanExecute(true);
            }
        }

        private RelayCommand addDetails;
        public RelayCommand AddDetails
        {
            get
            {
                return addDetails ??
                 (addDetails = new RelayCommand(obj =>
                 {
                     WindowNewDetails wnDetails = new WindowNewDetails
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdDetails = MaxId() + 1;
                     Details client = new Details { DetailsID = maxIdDetails };
                     wnDetails.DataContext = client;
                     if (wnDetails.ShowDialog() == true)
                     {
                         ListDetails.Add(client);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<Details>(client);
                     }
                     SelectedDetails = client;
                 }));
            }
        }

        private RelayCommand deleteDetails;
        public RelayCommand DeleteDetails
        {
            get
            {
                return deleteDetails ??
                (deleteDetails = new RelayCommand(obj =>
                {
                    Details client = SelectedDetails;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные детали по названию: " + client.DetailsName, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            DetailsID = selectedDetails.DetailsID
                        };
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            dbContext.Archive.Add(archiveEntry);
                            dbContext.SaveChanges();
                        }
                    }
                }, (obj) => SelectedDetails != null && ListDetails.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}