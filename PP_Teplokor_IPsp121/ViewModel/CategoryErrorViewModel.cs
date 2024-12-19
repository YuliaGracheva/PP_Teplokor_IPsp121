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
    public class CategoryErrorViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CategoryError> ListCategoryError { get; set; }
        public static int CategoryErrorID;
        public int MaxId()
        {
            int max = 0;
            if (this.ListCategoryError != null)
            {
                foreach (var cl in this.ListCategoryError)
                {
                    if (max < cl.CategoryErrorID) max = cl.CategoryErrorID;
                }
            }
            return max;
        }
        public ObservableCollection<string> MenuItems { get; set; }
        public CategoryErrorViewModel(CurrentUser currentUser)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<CategoryError> clients = MyDbContext.GetEntities<CategoryError>(connectionString, "SELECT * FROM CategoryError cl LEFT JOIN Archive a ON a.CategoryErrorID = cl.CategoryErrorID WHERE a.ArchiveID IS NULL;");

            // Преобразование в ObservableCollection
            ListCategoryError = new ObservableCollection<CategoryError>(clients);
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

        private RelayCommand editCategoryError;
        public RelayCommand EditCategoryError
        {
            get
            {
                return editCategoryError ??
                (editCategoryError = new RelayCommand(obj =>
                {
                    WindowNewCategoryError wnCategoryError = new WindowNewCategoryError
                    { Title = "Редактирование сотрудника" };

                    CategoryError client = SelectedCategoryError;
                    CategoryError tempClient = new CategoryError();

                    tempClient = client.ShallowCopy();
                    wnCategoryError.DataContext = tempClient;
                    if (wnCategoryError.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        client.CategoryErrorName = tempClient.CategoryErrorName;
                        client.CategoryErrorDescriprion = tempClient.CategoryErrorDescriprion;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<CategoryError>(tempClient);
                    }
                }, (obj) => SelectedCategoryError != null && ListCategoryError.Count > 0));
            }
        }


        private CategoryError selectedCategoryError;
        public CategoryError SelectedCategoryError
        {
            get
            {
                return selectedCategoryError;
            }
            set
            {
                selectedCategoryError = value;
                OnPropertyChanged("SelectedCategoryError");
                EditCategoryError.CanExecute(true);
            }
        }

        private RelayCommand addCategoryError;
        public RelayCommand AddCategoryError
        {
            get
            {
                return addCategoryError ??
                 (addCategoryError = new RelayCommand(obj =>
                 {
                     WindowNewCategoryError wnClient = new WindowNewCategoryError
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdClient = MaxId() + 1;
                     CategoryError client = new CategoryError { CategoryErrorID = maxIdClient };
                     wnClient.DataContext = client;
                     if (wnClient.ShowDialog() == true)
                     {
                         ListCategoryError.Add(client);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<CategoryError>(client);
                     }
                     SelectedCategoryError = client;
                 }));
            }
        }

        private RelayCommand deleteCategoryError;
        public RelayCommand DeleteCategoryError
        {
            get
            {
                return deleteCategoryError ??
                (deleteCategoryError = new RelayCommand(obj =>
                {
                    CategoryError client = SelectedCategoryError;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные ошибки по названию: " + client.CategoryErrorName, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            CategoryErrorID = selectedCategoryError.CategoryErrorID
                        };
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            dbContext.Archive.Add(archiveEntry);
                            dbContext.SaveChanges();
                        }
                    }
                }, (obj) => SelectedCategoryError != null && ListCategoryError.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}