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
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> ListClient { get; set; }
        public ObservableCollection<Address> ListAddress { get; set; }
        public static int ClientID;
        public int MaxId()
        {
            int max = 0;
            if (this.ListClient != null)
            {
                foreach (var cl in this.ListClient)
                {
                    if (max < cl.ClientID) max = cl.ClientID;
                }
            }
            return max;
        }
        public static int AddressID;
        public int MaxadId()
        {
            int max = 0;
            if (this.ListAddress != null)
            {
                foreach (var cl in this.ListAddress)
                {
                    if (max < cl.AddressID) max = cl.AddressID;
                }
            }
            return max;
        }

        public ObservableCollection<ApplicationDetails> ListApplicationDetails { get; set; }
        public ObservableCollection<ApplicationDiagnosticsDPO> ListApplicationDiagnosticsDPO { get; set; }
        public static int ApplicationDetailsID;
        public int MaxappdId()
        {
            int max = 0;
            if (this.ListApplicationDetails != null)
            {
                foreach (var cl in this.ListApplicationDetails)
                {
                    if (max < cl.ApplicationDetailsID) max = cl.ApplicationDetailsID;
                }
            }
            return max;
        }
        public ObservableCollection<Distribution> ListDistribution { get; set; }
        public static int DistributionID;
        public int MaxdistribId()
        {
            int max = 0;
            if (this.ListDistribution != null)
            {
                foreach (var cl in this.ListDistribution)
                {
                    if (max < cl.DistributionID) max = cl.DistributionID;
                }
            }
            return max;
        }
        public ObservableCollection<ApplicationDiagnostics> ListApplicationDiagnostics { get; set; }
        public static int ApplicationDiagnosticsID;

        public int MaxapdisId()
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
        private readonly MyDbContext _context;

        // Список клиентов, категорий ошибок и адресов
        public List<Client> Clients { get; private set; }
        public List<CategoryError> CategoryErrors { get; private set; }
        public List<Address> Addresses { get; private set; }


        // Метод для загрузки данных из базы
        private void LoadData()
        {
            Clients = _context.Client.ToList();
            CategoryErrors = _context.CategoryError.ToList();
            Addresses = _context.Address.ToList();
        }

        // Метод для получения имени клиента по ID
        public string GetClientName(int clientId)
        {
            var client = Clients.FirstOrDefault(c => c.ClientID == clientId);
            return client?.ClientNumber ?? "Неизвестный клиент";
        }

        // Метод для получения категории ошибки по ID
        public string GetCategoryErrorName(int categoryErrorId)
        {
            var category = CategoryErrors.FirstOrDefault(e => e.CategoryErrorID == categoryErrorId);
            return category?.CategoryErrorName ?? "Неизвестная ошибка";
        }

        // Метод для получения адреса клиента по ID
        public string GetAddressClient(int addressClientId)
        {
            var address = Addresses.FirstOrDefault(a => a.AddressID == addressClientId);
            return address?.AddressCity ?? "Неизвестный адрес";
        }
        public ObservableCollection<CategoryError> ListCategoryError { get; set; }
        public ObservableCollection<string> MenuItems { get; set; }
        public ClientViewModel(CurrentUser currentUsers)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";
            MyDbContext context = new MyDbContext();
            _context = context;

            // Загружаем данные для всех клиентов, ошибок и адресов при создании ViewModel
            LoadData();
            // Получаем данные из базы данных
            List<CategoryError> eror = MyDbContext.GetEntities<CategoryError>(connectionString, "SELECT * FROM CategoryError  cl LEFT JOIN Archive a ON a.CategoryErrorID = cl.CategoryErrorID WHERE a.ArchiveID IS NULL;");
            List<Client> clients = MyDbContext.GetEntities<Client>(connectionString, "SELECT * FROM Client  cl LEFT JOIN Archive a ON a.ClientID = cl.ClientID WHERE a.ArchiveID IS NULL;");
            List<Address> employee = MyDbContext.GetEntities<Address>(connectionString, "SELECT * FROM Address cl LEFT JOIN Archive a ON a.AddressID = cl.AddressID WHERE a.ArchiveID IS NULL;");

            // Преобразуем данные в ObservableCollection
            error = eror ?? new List<CategoryError>(); // В случае, если данные не загружены, присваиваем пустой список
            client = clients ?? new List<Client>();     // Аналогично для клиентов
            address = employee ?? new List<Address>();  // И для адресов

            ListClient = new ObservableCollection<Client>(clients ?? new List<Client>());

            currentUser = currentUsers;
            // Преобразование в ObservableCollection
            ListAddress = new ObservableCollection<Address>(employee);

            List<ApplicationDetails> appliDetails = MyDbContext.GetEntities<ApplicationDetails>(connectionString, "SELECT * FROM ApplicationDetails  cl LEFT JOIN Archive a ON a.ApplicationDetailsID = cl.ApplicationDetailsID WHERE a.ArchiveID IS NULL;");

            // Преобразование в ObservableCollection
            ListApplicationDetails = new ObservableCollection<ApplicationDetails>(appliDetails);

            string query = "SELECT d.*" +
               "FROM Distribution d LEFT JOIN Archive a ON a.DistributionID = d.DistributionID WHERE a.ArchiveID IS NULL;";

            List<Distribution> distributions = MyDbContext.GetEntities<Distribution>(connectionString, query);
            ListDistribution = new ObservableCollection<Distribution>(distributions);

            List<ApplicationDiagnostics> applicationDiagnostics = MyDbContext.GetEntities<ApplicationDiagnostics>(connectionString, "SELECT * FROM ApplicationDiagnostics  cl LEFT JOIN Archive a ON a.ApplicationDiagnosticsID = cl.ApplicationDiagnosticsID WHERE a.ArchiveID IS NULL;");
            LoadEmployees();
            // Преобразование в ObservableCollection
            ListApplicationDiagnostics = new ObservableCollection<ApplicationDiagnostics>(applicationDiagnostics);

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
        private void LoadEmployees()
        {
            WindowNewDistribution windowNewDistribution = new WindowNewDistribution();
            MyDbContext dbContext = new MyDbContext();
            employees = new ObservableCollection<Employee>(
                dbContext.Employee.Where(e => e.EmployeeRoleID == 2).ToList()
            );
            windowNewDistribution.TbMaster.ItemsSource = employees;
        }
        private RelayCommand editClient;
        public RelayCommand EditClient
        {
            get
            {
                return editClient ??
                (editClient = new RelayCommand(obj =>
                {
                    if (!currentUser.IsAdmin)
                    {
                        WindowNewClient wnClient = new WindowNewClient
                        { Title = "Редактирование сотрудника" };

                        Client client = SelectedClient;
                        Client tempClient = new Client();

                        tempClient = client.ShallowCopy();
                        wnClient.DataContext = tempClient;
                        if (wnClient.ShowDialog() == true)
                        {
                            // сохранение данных в оперативной памяти
                            client.ClientSurname = tempClient.ClientSurname;
                            client.ClientName = tempClient.ClientName;
                            client.ClientPatronymic = tempClient.ClientPatronymic;
                            client.ClientNumber = tempClient.ClientNumber;
                            MyDbContext dbContext = new MyDbContext();
                            dbContext.UpdateEntity<Client>(tempClient);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedClient != null && ListClient.Count > 0));
            }
        }


        private Client selectedClient;
        public Client SelectedClient
        {
            get
            {
                return selectedClient;
            }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
                EditClient.CanExecute(true);
            }
        }

        private RelayCommand addClient;
        public RelayCommand AddClient
        {
            get
            {
                return addClient ??
                 (addClient = new RelayCommand(obj =>
                 {
                     if(!currentUser.IsAdmin) { 
                     WindowNewClient wnClient = new WindowNewClient
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdClient = MaxId() + 1;
                     Client client = new Client { ClientID = maxIdClient };
                     wnClient.DataContext = client;
                     if (wnClient.ShowDialog() == true)
                     {
                         ListClient.Add(client);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<Client>(client);
                     }
                     SelectedClient = client;
                     }
                     else
                     {
                         MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                 }));
            }
        }

        private RelayCommand deleteClient;
        public RelayCommand DeleteClient
        {
            get
            {
                return deleteClient ??
                (deleteClient = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    Client client = SelectedClient;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные клиента по фамилии: " + client.ClientSurname + ". А также все сопотствующие ему данные. ", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        using (MyDbContext dbContext = new MyDbContext())
                        {
                            // Архивируем данные клиента
                            Archive archiveEntry = new Archive
                            {
                                ClientID = client.ClientID
                            };
                            dbContext.Archive.Add(archiveEntry);

                            var addresses = dbContext.Address.Where(a => a.ClientID == client.ClientID).ToList();
                            foreach (var address in addresses)
                            {
                                Archive archiveAddress = new Archive
                                {
                                    AddressID = address.AddressID, 
                                    ClientID = client.ClientID
                                };
                                dbContext.Archive.Add(archiveAddress);
                            }

                            // Получаем и архивируем диагностику приложений клиента
                            var diagnostics = dbContext.ApplicationDiagnostics.Where(ad => ad.ClientID == client.ClientID).ToList();
                            foreach (var diagnostic in diagnostics)
                            {
                                // Добавляем только ID диагностики в архив
                                Archive archiveDiagnostic = new Archive
                                {
                                    ApplicationDiagnosticsID = diagnostic.ApplicationDiagnosticsID,
                                    ClientID = client.ClientID
                                };
                                dbContext.Archive.Add(archiveDiagnostic);
                            }

                            // Сохраняем все изменения в базе данных
                            dbContext.SaveChanges();
                        }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedClient != null && ListClient.Count > 0));
            }
        }
        private RelayCommand editApplicationDiagnostics;
        public RelayCommand EditApplicationDetails
        {
            get
            {
                return editApplicationDiagnostics ??
                (editApplicationDiagnostics = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    WindowNewApplicationDetails wnClient = new WindowNewApplicationDetails
                    { Title = "" };

                    ApplicationDetailsDPO сriminalPersonDPO = SelectedApplicationDetailsDPO;
                    var tempCriminalPerson = сriminalPersonDPO.ShallowCopy();
                    wnClient.DataContext = tempCriminalPerson;

                    ObservableCollection<Details> group = new ObservableCollection<Details>();

                    // Присвоение ItemsSource
                    wnClient.TbDetails.ItemsSource = group;

                    if (wnClient.ShowDialog() == true)
                    {
                        var prof = (Details)wnClient.TbDetails.SelectedValue;

                        if (prof != null)
                        {
                            сriminalPersonDPO.ApplicationDetailsID = tempCriminalPerson.ApplicationDetailsID;
                            сriminalPersonDPO.ApplicationDiagnosticsID = tempCriminalPerson.ApplicationDiagnosticsID;
                            сriminalPersonDPO.DetailsName =  prof.DetailsName;
                        }
                        FindApplicationDetails finder = new FindApplicationDetails(сriminalPersonDPO.ApplicationDetailsID);

                        List<ApplicationDetails> listCriminalPerson = ListApplicationDetails.ToList();
                        ApplicationDetails? ser = listCriminalPerson.Find(new Predicate<ApplicationDetails>(finder.CriminalPersonPredicate));

                        if (ser != null)
                        {
                            ser = ser.CopyFromClientEmployeeDPO(сriminalPersonDPO);
                            MyDbContext dbContext = new MyDbContext();
                            try
                            {
                                dbContext.UpdateEntity<ApplicationDetails>(ser);
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
                }, (obj) => SelectedApplicationDetails != null && ListApplicationDetails.Count > 0));
            }
        }
        private ApplicationDetailsDPO selectedCriminalPersonDPO;
        public ApplicationDetailsDPO SelectedApplicationDetailsDPO
        {
            get
            {
                return selectedCriminalPersonDPO;
            }
            set
            {
                selectedCriminalPersonDPO = value;
                OnPropertyChanged("SelectedApplicationDetailsDPO");
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
        public ObservableCollection<ApplicationDetailsDPO> ListApplicationDetailsDPO { get; set; }
        private RelayCommand addApplicationDiagnostics;
        public RelayCommand AddApplicationDetails
        {
            get
            {
                return addApplicationDiagnostics ??
                 (addApplicationDiagnostics = new RelayCommand(obj =>
                 {
                     if(!currentUser.IsAdmin) { 
                     WindowNewApplicationDetails wnClient = new WindowNewApplicationDetails
                     {
                         Title = "",
                     };
                     ApplicationDetailsDPO criminalPersonDPO = new ApplicationDetailsDPO();
                     wnClient.DataContext = criminalPersonDPO;

                     ObservableCollection<Details> groupp = new ObservableCollection<Details>();
                     // Присвоение ItemsSource
                     wnClient.TbDetails.ItemsSource = groupp;

                     if (wnClient.ShowDialog() == true)
                     {
                         Details group = (Details)wnClient.TbDetails.SelectedValue;

                         criminalPersonDPO.DetailsName = group.DetailsName;

                         try
                         {
                             using (MyDbContext dbContext = new MyDbContext())
                             {
                                 ListApplicationDetailsDPO.Add(criminalPersonDPO);
                                 ApplicationDetails criminalPerson = new ApplicationDetails();
                                 criminalPerson = criminalPerson.CopyFromClientEmployeeDPO(criminalPersonDPO);
                                 dbContext.ApplicationDetails.Add(criminalPerson);
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
                    if (!currentUser.IsAdmin) { 
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
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedApplicationDetails != null && ListApplicationDetails.Count > 0));
            }
        }

        private RelayCommand editAddress;
        public RelayCommand EditAddress
        {
            get
            {
                return editAddress ??
                (editAddress = new RelayCommand(obj =>
                {
                    if (!currentUser.IsAdmin) { 
                    WindowNewAddress wnEmployee = new WindowNewAddress
                    { Title = "Редактирование сотрудника" };

                    Address employee = SelectedAddress;
                    Address tempEmployee = new Address();

                    tempEmployee = employee.ShallowCopy();
                    wnEmployee.DataContext = tempEmployee;
                    if (wnEmployee.ShowDialog() == true)
                    {
                        // сохранение данных в оперативной памяти
                        employee.AddressCity = tempEmployee.AddressCity;
                        employee.AddressNumber = tempEmployee.AddressNumber;
                        employee.AddressHome = tempEmployee.AddressHome;
                        employee.AddressComment = tempEmployee.AddressComment;
                        employee.AddressStreet = tempEmployee.AddressStreet;
                        MyDbContext dbContext = new MyDbContext();
                        dbContext.UpdateEntity<Address>(tempEmployee);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedAddress != null && ListAddress.Count > 0));
            }
        }


        private Address selectedAddress;
        public Address SelectedAddress
        {
            get
            {
                return selectedAddress;
            }
            set
            {
                selectedAddress = value;
                OnPropertyChanged("SelectedAddress");
                EditAddress.CanExecute(true);
            }
        }

        private RelayCommand addAddress;
        public RelayCommand AddAddress
        {
            get
            {
                return addAddress ??
                 (addAddress = new RelayCommand(obj =>
                 {
                     if(!currentUser.IsAdmin) { 
                     WindowNewAddress wnEmployee = new WindowNewAddress
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdAddress = MaxadId() + 1;
                     Address employee = new Address { AddressID = maxIdAddress };
                     wnEmployee.DataContext = employee;
                     if (wnEmployee.ShowDialog() == true)
                     {
                         ListAddress.Add(employee);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<Address>(employee);
                     }
                     SelectedAddress = employee;
                     }
                     else
                     {
                         MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                 }));
            }
        }

        private RelayCommand deleteAddress;
        public RelayCommand DeleteAddress
        {
            get
            {
                return deleteAddress ??
                (deleteAddress = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    Address employee = SelectedAddress;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив адрес: г. " + selectedAddress.AddressCity + ", ул. " + selectedAddress.AddressStreet + ", д. " + selectedAddress.AddressHome + ", кв. " + selectedAddress.AddressNumber, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            AddressID = selectedAddress.AddressID
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
                }, (obj) => SelectedAddress != null && ListAddress.Count > 0));
            }
        }
        private RelayCommand editDistribution;
        public RelayCommand EditDistribution
        {
            get
            {
                return editDistribution ??
                (editDistribution = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    WindowNewDistribution wnDistribution = new WindowNewDistribution
                    { Title = "" };

                    DistributionDPO сriminalPersonDPO = SelectedDistributionDPO;
                    var tempCriminalPerson = сriminalPersonDPO.ShallowCopy();
                    wnDistribution.DataContext = tempCriminalPerson;

                    ObservableCollection<Employee> group = new ObservableCollection<Employee>();

                    // Присвоение ItemsSource
                    wnDistribution.TbMaster.ItemsSource = group;

                    if (wnDistribution.ShowDialog() == true)
                    {
                        var prof = (Employee)wnDistribution.TbMaster.SelectedValue;

                        if (prof != null)
                        {
                            сriminalPersonDPO.DistributionDate = tempCriminalPerson.DistributionDate;
                            сriminalPersonDPO.DistributionTime = tempCriminalPerson.DistributionTime;
                            сriminalPersonDPO.DistributionID = tempCriminalPerson.DistributionID;
                            сriminalPersonDPO.ApplicationDiagnosticsID = tempCriminalPerson.DistributionID;
                            сriminalPersonDPO.EmployeeSurname = prof.EmployeeSurname;
                        }
                        FindDestribution finder = new FindDestribution(сriminalPersonDPO.DistributionID);

                        List<Distribution> listCriminalPerson = ListDistribution.ToList();
                        Distribution? ser = listCriminalPerson.Find(new Predicate<Distribution>(finder.CriminalPersonPredicate));

                        if (ser != null)
                        {
                            ser = ser.CopyFromClientEmployeeDPO(сriminalPersonDPO);
                            MyDbContext dbContext = new MyDbContext();
                            try
                            {
                                dbContext.UpdateEntity<Distribution>(ser);
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
                }, (obj) => SelectedDistributionDPO != null && ListDistribution.Count > 0));
            }
        }
        private DistributionDPO selectedDistributionDPO;
        public DistributionDPO SelectedDistributionDPO
        {
            get
            {
                return selectedDistributionDPO;
            }
            set
            {
                selectedDistributionDPO = value;
                OnPropertyChanged("SelectedDistributionDPO");
            }
        }


        private Distribution selectedDistribution;
        public Distribution SelectedDistribution
        {
            get
            {
                return selectedDistribution;
            }
            set
            {
                selectedDistribution = value;
                OnPropertyChanged("SelectedDistribution");
                EditDistribution.CanExecute(true);
            }
        }
        private ObservableCollection<Employee> employees;
        private RelayCommand addDistribution;
        public RelayCommand AddDistribution
        {
            get
            {
                return addDistribution ??
                    (addDistribution = new RelayCommand(obj =>
                    {
                        if (!currentUser.IsAdmin) { 
                        // Ваш существующий код для открытия окна и сохранения данных
                        WindowNewDistribution wnClient = new WindowNewDistribution
                        {
                            Title = "Новый сотрудник",
                        };
                        int maxIdClient = MaxId() + 1;
                        Distribution client = new Distribution { DistributionID = maxIdClient };
                        wnClient.DataContext = client;
                        if (wnClient.ShowDialog() == true)
                        {
                            ListDistribution.Add(client);
                            MyDbContext dbContext = new MyDbContext();
                            dbContext.SaveEntity<Distribution>(client);
                        }
                        SelectedDistribution = client;
                        }
                        else
                        {
                            MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }, obj => SelectedDistribution != null)); // Команда активна только если выбран элемент
            }
        }


        private RelayCommand deleteDistribution;
        public RelayCommand DeleteDistribution
        {
            get
            {
                return deleteDistribution ??
                (deleteDistribution = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    Distribution client = SelectedDistribution;
                    MessageBoxResult result = MessageBox.Show("Перенести в архив данные распределения по дате и времени: " + client.DistributionDate + ", " + client.DistributionTime, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        Archive archiveEntry = new Archive
                        {
                            DistributionID = selectedDistribution.DistributionID
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
                }, (obj) => SelectedDistribution != null && ListDistribution.Count > 0));
            }
        }

        public ObservableCollection<ApplicationDiagnosticsDPO> GetListCriminalPersonDPO()
        {
            foreach (var ser in ListApplicationDiagnostics)
            {
                ApplicationDiagnosticsDPO criminalPersonDPO = new Model.ApplicationDiagnosticsDPO();
                criminalPersonDPO = criminalPersonDPO.CopyFromEmployee(ser);
                ListApplicationDiagnosticsDPO.Add(criminalPersonDPO);
            }
            return ListApplicationDiagnosticsDPO;
        }

        private List<Address> group;
        private List<Client> status;
        private List<CategoryError> proffession;
        private CurrentUser currentUser;
        private RelayCommand editCriminalPerson;
        public RelayCommand EditApplicationDiagnostics
        {
            get
            {
                return editCriminalPerson ?? (editCriminalPerson = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    WindowNewApplicationDiagnostics wnCriminalPerson = new WindowNewApplicationDiagnostics()
                    {
                        Title = ""
                    };
                    wnCriminalPerson.DataContext = currentUser;
                    ApplicationDiagnosticsDPO сriminalPersonDPO = SelectedApplicationDiagnosticsDPO;
                    var tempCriminalPerson = сriminalPersonDPO.ShallowCopy();
                    wnCriminalPerson.DataContext = tempCriminalPerson;

                    // Инициализация ObservableCollection
                    ObservableCollection<CategoryError> group = new ObservableCollection<CategoryError>(error);
                    ObservableCollection<Address> status = new ObservableCollection<Address>(address);
                    ObservableCollection<Client> proffession = new ObservableCollection<Client>(client);

                    // Присвоение ItemsSource
                    wnCriminalPerson.TbCategoreError.ItemsSource = group;
                    wnCriminalPerson.TbAddress.ItemsSource = status;
                    wnCriminalPerson.TbClient.ItemsSource = proffession;

                    if (wnCriminalPerson.ShowDialog() == true)
                    {
                        var prof = (Address)wnCriminalPerson.TbAddress.SelectedValue;
                        var stat = (CategoryError)wnCriminalPerson.TbCategoreError.SelectedValue;
                        var groupp = (Client)wnCriminalPerson.TbClient.SelectedValue;

                        if (prof != null && stat != null && groupp != null)
                        {
                            сriminalPersonDPO.ApplicationDiagnosticsID = tempCriminalPerson.ApplicationDiagnosticsID;
                            сriminalPersonDPO.ApplicationDiagnosticsPrice = tempCriminalPerson.ApplicationDiagnosticsPrice;
                            сriminalPersonDPO.ApplicationDiagnosticsComment = tempCriminalPerson.ApplicationDiagnosticsComment;
                            сriminalPersonDPO.AddressClient = prof.AddressCity + ", ул. " + prof.AddressStreet + ", д. " + prof.AddressHome + ", кв. " + prof.AddressNumber;
                            сriminalPersonDPO.ClientSurname = groupp.ClientNumber;
                            сriminalPersonDPO.CategoryErrorName = stat.CategoryErrorName;
                        }

                        FindApplicationDiagnostics finder = new FindApplicationDiagnostics(сriminalPersonDPO.ApplicationDiagnosticsID);

                        List<ApplicationDiagnostics> listCriminalPerson = ListApplicationDiagnostics.ToList();
                        ApplicationDiagnostics? ser = listCriminalPerson.Find(new Predicate<ApplicationDiagnostics>(finder.CriminalPersonPredicate));

                        if (ser != null)
                        {
                            ser = ser.CopyFromClientEmployeeDPO(сriminalPersonDPO);
                            MyDbContext dbContext = new MyDbContext();
                            try
                            {
                                dbContext.UpdateEntity<ApplicationDiagnostics>(ser);
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
                }, (obj) => SelectedApplicationDiagnosticsDPO != null && ListApplicationDiagnostics.Count > 0));
            }
        }

        private ApplicationDiagnosticsDPO selectedApplicationDiagnosticas;
        public ApplicationDiagnosticsDPO SelectedApplicationDiagnosticsDPO
        {
            get
            {
                return selectedApplicationDiagnosticas;
            }
            set
            {
                selectedApplicationDiagnosticas = value;
                OnPropertyChanged("SelectedApplicationDiagnosticsDPO");
                EditApplicationDiagnostics.CanExecute(true);
            }
        }
        
        private List<CategoryError> error = new List<CategoryError>();  // Инициализация
        private List<Client> client = new List<Client>();  // Инициализация
        private List<Address> address = new List<Address>();  // Инициализация
        private RelayCommand addApplicationDiagnosticas;
        public RelayCommand AddApplicationDiagnostics
        {
            get
            {
                return addApplicationDiagnosticas ?? (addApplicationDiagnosticas = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    // Создаем окно с новой диагностикой
                    WindowNewApplicationDiagnostics wnClient = new WindowNewApplicationDiagnostics()
                    {
                        Title = "Новая диагностика",
                    };

                    ApplicationDiagnosticsDPO сriminalPersonDPO = new ApplicationDiagnosticsDPO();
                    wnClient.DataContext = сriminalPersonDPO;
                    wnClient.DataContext = currentUser; 
                    // Инициализация ObservableCollection с уникальными именами
                    // Инициализация ObservableCollection
                    ObservableCollection <CategoryError> categoryErrors = new ObservableCollection<CategoryError>(error);
                    ObservableCollection<Address> addresses = new ObservableCollection<Address>(address);
                    ObservableCollection<Client> clients = new ObservableCollection<Client>(client);
                    wnClient.TbCategoreError.ItemsSource = categoryErrors;
                    wnClient.TbAddress.ItemsSource = addresses;
                    wnClient.TbClient.ItemsSource = clients;

                    // Присвоение ItemsSource
                    wnClient.TbCategoreError.ItemsSource = categoryErrors;
                    wnClient.TbAddress.ItemsSource = addresses;
                    wnClient.TbClient.ItemsSource = clients;

                    // Отображение окна
                    if (wnClient.ShowDialog() == true)
                    {
                        // Получение выбранных значений
                        Client selectedClient = (Client)wnClient.TbClient.SelectedValue;
                        Address selectedAddress = (Address)wnClient.TbAddress.SelectedValue;
                        CategoryError selectedCategoryError = (CategoryError)wnClient.TbCategoreError.SelectedValue;

                        // Присвоение значений в DPO объект
                        сriminalPersonDPO.ClientSurname = selectedClient.ClientNumber;
                        сriminalPersonDPO.AddressClient = selectedAddress.AddressCity;
                        сriminalPersonDPO.CategoryErrorName = selectedCategoryError.CategoryErrorName;

                        try
                        {
                            // Добавление нового объекта в список
                            ApplicationDiagnostics criminalPerson = new ApplicationDiagnostics();
                            criminalPerson = criminalPerson.CopyFromClientEmployeeDPO(сriminalPersonDPO);

                            // Сохранение в базу данных
                            using (MyDbContext dbContext = new MyDbContext())
                            {
                                dbContext.ApplicationDiagnostics.Add(criminalPerson);
                                dbContext.SaveChanges();
                            }

                            MessageBox.Show("Диагностика сохранена успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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


        private RelayCommand deleteApplicationDiagnosticas;
        public RelayCommand DeleteApplicationDiagnostics
        {
            get
            {
                return deleteApplicationDiagnosticas ??
                (deleteApplicationDiagnosticas = new RelayCommand(obj =>
                {
                    if(!currentUser.IsAdmin) { 
                    ApplicationDiagnosticsDPO client = SelectedApplicationDiagnosticsDPO;
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
                    }
                    else
                    {
                        MessageBox.Show("Нет права изменения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, (obj) => SelectedApplicationDiagnosticsDPO != null && ListApplicationDiagnostics.Count > 0));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}