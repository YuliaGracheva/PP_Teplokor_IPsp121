using Microsoft.EntityFrameworkCore;
using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
using PP_Teplokor_IPsp121.View.New;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> ListEmployee { get; set; }
        public static int EmployeeID;
        public ObservableCollection<string> MenuItems { get; set; }
        public int MaxId()
        {
            int max = 0;
            if (this.ListEmployee != null)
            {
                foreach (var cl in this.ListEmployee)
                {
                    if (max < cl.EmployeeID) max = cl.EmployeeID;
                }
            }
            return max;
        }
        private CurrentUser CurrentUser { get; set; }
        public EmployeeViewModel(CurrentUser currentUser)
        {
            CurrentUser = currentUser;
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<Employee> employee = MyDbContext.GetEntities<Employee>(connectionString, "SELECT * FROM Employee WHERE EmployeeID NOT IN (SELECT EmployeeID FROM Archive)");

            // Преобразование в ObservableCollection
            ListEmployee = new ObservableCollection<Employee>(employee);
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
        public RelayCommand EditEmployee
        {
            get
            {
                return editEmployee ??
                (editEmployee = new RelayCommand(obj =>
                {
                    if (!CurrentUser.IsAdmin) { 
                    WindowNewEmployee wnEmployee = new WindowNewEmployee
                    { Title = "" };

                    EmployeeDPO сriminalPersonDPO = SelectedEmployeeDPO;
                    var tempCriminalPerson = сriminalPersonDPO.ShallowCopy();
                    wnEmployee.DataContext = tempCriminalPerson;

                    ObservableCollection<EmployeeRole> group = new ObservableCollection<EmployeeRole>();

                    // Присвоение ItemsSource
                    wnEmployee.TbEmployeeRole.ItemsSource = group; 

                    if (wnEmployee.ShowDialog() == true)
                    {
                        var prof = (EmployeeRole)wnEmployee.TbEmployeeRole.SelectedValue;

                        if (prof != null)
                        {
                            сriminalPersonDPO.EmployeeSurname = tempCriminalPerson.EmployeeSurname;
                            сriminalPersonDPO.EmployeePassword = tempCriminalPerson.EmployeePassword;
                            сriminalPersonDPO.EmployeeName = tempCriminalPerson.EmployeeName;
                            сriminalPersonDPO.EmployeePatronymic = tempCriminalPerson.EmployeePatronymic;
                            сriminalPersonDPO.EmployeeLogin = tempCriminalPerson.EmployeeLogin;
                            сriminalPersonDPO.EmployeeRoleName = prof.EmployeeRoleName;
                        }
                        FindEmployee finder = new FindEmployee(сriminalPersonDPO.EmployeeID);

                        List<Employee> listCriminalPerson = ListEmployee.ToList();
                        Employee? ser = listCriminalPerson.Find(new Predicate<Employee>(finder.CriminalPersonPredicate));

                        if (ser != null)
                        {
                            ser = ser.CopyFromClientEmployeeDPO(сriminalPersonDPO);
                            MyDbContext dbContext = new MyDbContext();
                            try
                            {
                                dbContext.UpdateEntity<Employee>(ser);
                            }
                            catch (Exception e)
                            {
                                string Error = "Ошибка добавления данных в бд\n" + e.Message;
                            }
                        }
                        else
                        {
                            // Обработка ситуации, когда элемент не найден
                            MessageBox.Show("Элемент не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedEmployeeDPO != null && ListEmployee.Count > 0));
            }
        }

        private EmployeeDPO selectedCriminalPersonDPO;
        public EmployeeDPO SelectedEmployeeDPO
        {
            get
            {
                return selectedCriminalPersonDPO;
            }
            set
            {
                selectedCriminalPersonDPO = value;
                OnPropertyChanged("SelectedEmployeeDPO");
            }
        }

        private Employee selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            {
                return selectedEmployee;
            }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
                EditEmployee.CanExecute(true);
            }
        }
        public ObservableCollection<EmployeeDPO> ListEmployeeDPO { get; set; }
        private RelayCommand addEmployee;
        public RelayCommand AddEmployee
        {
            get
            {
                return addEmployee ??
                 (addEmployee = new RelayCommand(obj =>
                 {
                     if(!CurrentUser.IsAdmin) { 
                     WindowNewEmployee wnEmployee = new WindowNewEmployee
                     {
                         Title = "",
                     };
                     EmployeeDPO criminalPersonDPO = new EmployeeDPO();
                     wnEmployee.DataContext = criminalPersonDPO;

                     ObservableCollection<EmployeeRole> groupp = new ObservableCollection<EmployeeRole>();

                     // Присвоение ItemsSource
                     wnEmployee.TbEmployeeRole.ItemsSource = groupp; 

                     if (wnEmployee.ShowDialog() == true)
                     {
                         EmployeeRole group = (EmployeeRole)wnEmployee.TbEmployeeRole.SelectedValue;

                         criminalPersonDPO.EmployeeRoleName = group.EmployeeRoleName;

                         try
                         {
                             using (MyDbContext dbContext = new MyDbContext())
                             {
                                 ListEmployeeDPO.Add(criminalPersonDPO);
                                 Employee criminalPerson = new Employee();
                                 criminalPerson = criminalPerson.CopyFromClientEmployeeDPO(criminalPersonDPO);
                                 dbContext.Employee.Add(criminalPerson);
                                 dbContext.SaveChanges();
                             }
                         }
                         catch (DbUpdateException ex)
                         {
                             var innerException = ex.InnerException;
                             var errorMessage = ex.Message;

                             var messageText = "Ошибка при сохранении изменений:\n\n" +
                                                errorMessage + "\n\n" +
                                                "Внутреннее исключение:\n\n" +
                                                (innerException != null ? innerException.Message : "Нет дополнительной информации");

                             // Отображение сообщения об ошибке
                             MessageBox.Show(messageText, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                         }

                         }
                     }
                     else
                     {
                         MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                 }));
            }
        }

        private RelayCommand deleteEmployee;
        public RelayCommand DeleteEmployee
        {
            get
            {
                return deleteEmployee ??
                (deleteEmployee = new RelayCommand(obj =>
                {
                    if(!CurrentUser.IsAdmin) { 
                    Employee employee = SelectedEmployee;
                    MessageBoxResult result = MessageBox.Show("Удалить данные сотрудника по фамилии: " + employee.EmployeeSurname, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            EmployeeID = selectedEmployee.EmployeeID
                        };
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            dbContext.Archive.Add(archiveEntry);
                            dbContext.SaveChanges();
                        }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedEmployee != null && ListEmployee.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
