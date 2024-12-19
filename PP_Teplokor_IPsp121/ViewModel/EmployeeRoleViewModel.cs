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
    public class EmployeeRoleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EmployeeRole> ListEmployeeRole { get; set; }
        public static int EmployeeRoleID;
        public ObservableCollection<string> MenuItems { get; set; }
        public int MaxId()
        {
            int max = 0;
            if (this.ListEmployeeRole != null)
            {
                foreach (var cl in this.ListEmployeeRole)
                {
                    if (max < cl.EmployeeRoleID) max = cl.EmployeeRoleID;
                }
            }
            return max;
        }

        public EmployeeRoleViewModel(CurrentUser currentUser)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<EmployeeRole> employee = MyDbContext.GetEntities<EmployeeRole>(connectionString, "SELECT * FROM EmployeeRole WHERE EmployeeRoleID NOT IN (SELECT EmployeeRoleID FROM Archive)");

            // Преобразование в ObservableCollection
            ListEmployeeRole = new ObservableCollection<EmployeeRole>(employee);
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

        private RelayCommand editEmployee;
        public RelayCommand EditEmployeeRole
        {
            get
            {
                return editEmployee ??
                (editEmployee = new RelayCommand(obj =>
                {
                    WindowNewEmployeeRole wnEmployee = new WindowNewEmployeeRole
                    { Title = "Редактирование сотрудника" };

                    EmployeeRole employee = SelectedEmployeeRole;
                    EmployeeRole tempEmployee = new EmployeeRole();

                    tempEmployee = employee.ShallowCopy();
                    wnEmployee.DataContext = tempEmployee;
                    if (wnEmployee.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        employee.EmployeeRoleName = tempEmployee.EmployeeRoleName;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<EmployeeRole>(tempEmployee);
                    }
                }, (obj) => SelectedEmployeeRole != null && ListEmployeeRole.Count > 0));
            }
        }


        private EmployeeRole selectedEmployee;
        public EmployeeRole SelectedEmployeeRole
        {
            get
            {
                return selectedEmployee;
            }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployeeRole");
                EditEmployeeRole.CanExecute(true);
            }
        }

        private RelayCommand addEmployee;
        public RelayCommand AddEmployeeRole
        {
            get
            {
                return addEmployee ??
                 (addEmployee = new RelayCommand(obj =>
                 {
                     WindowNewEmployeeRole wnEmployee = new WindowNewEmployeeRole
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdEmployee = MaxId() + 1;
                     EmployeeRole employee = new EmployeeRole { EmployeeRoleID = maxIdEmployee };
                     wnEmployee.DataContext = employee;
                     if (wnEmployee.ShowDialog() == true)
                     {
                         ListEmployeeRole.Add(employee);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<EmployeeRole>(employee);
                     }
                     SelectedEmployeeRole = employee;
                 }));
            }
        }

        private RelayCommand deleteEmployee;
        public RelayCommand DeleteEmployeeRole
        {
            get
            {
                return deleteEmployee ??
                (deleteEmployee = new RelayCommand(obj =>
                {
                    EmployeeRole employee = SelectedEmployeeRole;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные должности по названию: " + employee.EmployeeRoleName, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            EmployeeRoleID = selectedEmployee.EmployeeRoleID
                        };
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            dbContext.Archive.Add(archiveEntry);
                            dbContext.SaveChanges();
                        }
                    }
                }, (obj) => SelectedEmployeeRole != null && ListEmployeeRole.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
