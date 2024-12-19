using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.Win32;
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
using System.Windows.Controls;
using System.Windows.Input;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class DistributionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Distribution> ListDistribution { get; set; }
        public static int DistributionID;
        public int MaxId()
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
        public ObservableCollection<DistributionItems> ListDistributionItems { get; set; }
        private readonly CurrentUser _currentUser;
        public ObservableCollection<string> MenuItems { get; set; }
        public DistributionViewModel(CurrentUser currentUser)
        {
                _currentUser = currentUser;
                string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

                DistributionItems = new ObservableCollection<DistributionItems>(GetDistributionItems());

                string query = "SELECT * FROM Distribution   cl LEFT JOIN Archive a ON a.DistributionID = cl.DistributionID WHERE a.ArchiveID IS NULL;";
                List<Distribution> clients = MyDbContext.GetEntities<Distribution>(connectionString, query);
                ListDistribution = new ObservableCollection<Distribution>(clients);

                // Инициализация команды
                CreateReportCommand = new RelayCommand(CreateReport);
            if (currentUser.IsAdmin)
            {
                MenuItems = new ObservableCollection<string>
            {
                "На главную",
                "Заявки",
                "Сотрудники",
                "Детали",
                "Категории ошибок",
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
        public ICommand CreateReportCommand { get; set; }
        public ObservableCollection<DistributionItems> DistributionItems { get; set; }

        private ObservableCollection<DistributionItems> GetDistributionItems()
        {
            string query = $"SELECT * FROM Distribution WHERE EmployeeID = {_currentUser.UserID}";

            // Получаем данные из БД с использованием фильтра по EmployeeID
            var distributions = MyDbContext.GetEntities<Distribution>(connectionString, query).ToList();

            var distributionItemsList = new ObservableCollection<DistributionItems>();

            foreach (var distribution in distributions)
            {

                var applicationDiagnostics = GetApplicationDiagnostics(distribution.ApplicationDiagnosticsID);
                if (applicationDiagnostics != null)
                {
                    var client = GetClient(applicationDiagnostics.ClientID);
                    var address = GetAddress(applicationDiagnostics.AddressID);
                    var employee = GetEmployee(distribution.EmployeeID);
                    var applicationDetails = GetApplicationDetails(applicationDiagnostics.ApplicationDiagnosticsID);

                    distributionItemsList.Add(new DistributionItems
                    {
                        Distribution = distribution,
                        ApplicationDiagnostics = applicationDiagnostics,
                        Client = client,
                        Address = address,
                        ApplicationDetails = applicationDetails,

                        DistributionDate = distribution.DistributionDate,
                        DistributionTime = distribution.DistributionTime,
                        EmployeeSurname = employee?.EmployeeSurname,
                        EmployeeName = employee?.EmployeeName,
                        ClientSurname = client?.ClientSurname,
                        ClientName = client?.ClientName,
                        ApplicationDiagnosticsID = applicationDiagnostics.ApplicationDiagnosticsID,
                        ApplicationDiagnosticsPrice = applicationDiagnostics.ApplicationDiagnosticsPrice,
                        AddressCity = address?.FullAddress
                    });
                }
            }

            return distributionItemsList;
        }


        // Добавьте этот метод в класс DistributionViewModel
        private Employee GetEmployee(int employeeID)
        {
            return MyDbContext.GetEntities<Employee>(connectionString, $"SELECT * FROM Employee WHERE EmployeeID = {employeeID}")
                .FirstOrDefault();
        }

        public string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";
        private IEnumerable<DistributionCardViewModel> GetDistributionCards()
        {
            var distributionCards = new List<DistributionCardViewModel>();

            // Получаем все распределения
            var distributions = MyDbContext.GetEntities<Distribution>(connectionString, "SELECT * FROM Distribution").ToList();

            foreach (var distribution in distributions)
            {
                var applicationDiagnostics = GetApplicationDiagnostics(distribution.ApplicationDiagnosticsID);
                if (applicationDiagnostics != null)
                {
                    var client = GetClient(applicationDiagnostics.ClientID);
                    var address = GetAddress(applicationDiagnostics.AddressID);
                    var applicationDetails = GetApplicationDetails(applicationDiagnostics.ApplicationDiagnosticsID);
                    var details = applicationDetails != null ? GetDetails(applicationDetails.DetailsID) : null;

                    distributionCards.Add(new DistributionCardViewModel
                    {
                        Distribution = distribution,
                        ApplicationDiagnostics = applicationDiagnostics,
                        Client = client,
                        Address = address,
                        ApplicationDetails = applicationDetails,
                        Details = details
                    });
                }
            }

            return distributionCards;
        }
        private ApplicationDiagnostics GetApplicationDiagnostics(int applicationDiagnosticsID)
        {
            return MyDbContext.GetEntities<ApplicationDiagnostics>(connectionString, $"SELECT * FROM ApplicationDiagnostics WHERE ApplicationDiagnosticsID = {applicationDiagnosticsID}")
                .FirstOrDefault();
        }

        private Client GetClient(int clientID)
        {
            return MyDbContext.GetEntities<Client>(connectionString, $"SELECT * FROM Client WHERE ClientID = {clientID}")
                .FirstOrDefault();
        }

        private Address GetAddress(int addressID)
        {
            return MyDbContext.GetEntities<Address>(connectionString, $"SELECT * FROM Address WHERE AddressID = {addressID}")
                .FirstOrDefault();
        }

        private ApplicationDetails GetApplicationDetails(int applicationDiagnosticsID)
        {
            return MyDbContext.GetEntities<ApplicationDetails>(connectionString, $"SELECT * FROM ApplicationDetails WHERE ApplicationDiagnosticsID = {applicationDiagnosticsID}")
                .FirstOrDefault();
        }

        private Details GetDetails(int detailsID)
        {
            return MyDbContext.GetEntities<Details>(connectionString, $"SELECT * FROM Details WHERE DetailsID = {detailsID}")
                .FirstOrDefault();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CreateReport(object parameter)
        {
            if (parameter is int distributionId && distributionId != 0)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Word Documents|*.docx",
                    Title = "Выберите место для сохранения отчета"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    CreateWordDocument(filePath, distributionId);  // Передаем DistributionID в метод
                }
            }
            else
            {
                MessageBox.Show("Не выбран ID распределения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Метод для создания Word-документа
        private void CreateWordDocument(string filePath, int distributionId)
        {
            // Получаем распределение по переданному ID
            var distribution = ListDistribution.FirstOrDefault(d => d.DistributionID == distributionId);
            if (distribution == null)
            {
                MessageBox.Show("Распределение не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var applicationDiagnostics = GetApplicationDiagnostics(distribution.ApplicationDiagnosticsID);
            var client = GetClient(applicationDiagnostics.ClientID);
            var address = GetAddress(applicationDiagnostics.AddressID);
            var employee = GetEmployee(distribution.EmployeeID);
            var applicationDetails = GetApplicationDetails(applicationDiagnostics.ApplicationDiagnosticsID);

            using (var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Сервисный центр
                var paragraph1 = new Paragraph(new Run(new Text("Сервисный-центр ТеплоКор")));
                paragraph1.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Left });
                body.Append(paragraph1);

                // ООО "Теплокор" и ИНН, ОГРН
                var paragraph2 = new Paragraph(new Run(new Text("ООО \"Теплокор\"\nИНН 3328488810, ОГРН 1133328001476")));
                paragraph2.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Left });
                body.Append(paragraph2);

                // Адрес
                var paragraph3 = new Paragraph(new Run(new Text("Адрес: ул. Гастелло, 8А (ТЦ Терминал, этаж 1, офис 101)")));
                paragraph3.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Left });
                body.Append(paragraph3);

                // Заявка на диагностику оборудования
                var paragraph4 = new Paragraph(new Run(new Text("Заявка на диагностику оборудование")));
                paragraph4.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                var run = new Run();
                run.RunProperties = new RunProperties(new Bold());
                run.AppendChild(new Text("Заявка на диагностику оборудование"));
                paragraph4.Append(run);
                body.Append(paragraph4);

                // Таблица с данными
                var table = new Table();

                // Дата распределения
                var row1 = new TableRow(
                    new TableCell(new Paragraph(new Run(new Text("Дата:")))),
                    new TableCell(new Paragraph(new Run(new Text(distribution.DistributionDate))))
                );
                table.Append(row1);

                // Заказчик
                var row2 = new TableRow(
                    new TableCell(new Paragraph(new Run(new Text("Заказчик:")))),
                    new TableCell(new Paragraph(new Run(new Text($"{client?.ClientSurname} {client?.ClientName}"))))
                );
                table.Append(row2);

                // Адрес заказчика
                var row3 = new TableRow(
                    new TableCell(new Paragraph(new Run(new Text("Адрес заказчика:")))),
                    new TableCell(new Paragraph(new Run(new Text(address?.FullAddress))))
                );
                table.Append(row3);

                // Добавление таблицы в документ
                body.Append(table);

                // Подписи
                var paragraph5 = new Paragraph(new Run(new Text("Подпись:")));
                body.Append(paragraph5);

                var paragraph6 = new Paragraph(new Run(new Text($"Исполнитель: {employee?.EmployeeSurname} {employee?.EmployeeName}")));
                body.Append(paragraph6);

                var paragraph7 = new Paragraph(new Run(new Text("Подпись:")));
                body.Append(paragraph7);
            }

            MessageBox.Show("Отчет создан успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}