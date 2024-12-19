using Microsoft.EntityFrameworkCore;
using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
using PP_Teplokor_IPsp121.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PP_Teplokor_IPsp121
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            var dbContext = new MyDbContext();
            dbContext.Database.EnsureCreated();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            using (MyDbContext db = new MyDbContext())
            {
                var employee = db.Employee.FromSqlRaw(
                    $"SELECT * FROM Employee WHERE EmployeeLogin = '{login}' AND EmployeePassword = '{password}'")
                    .FirstOrDefault();

                if (employee != null)
                {
                    // Используем существующий экземпляр CurrentUser через Singleton
                    CurrentUser currentUser = CurrentUser.Instance; // Используем Singleton
                    currentUser.SetRole(employee); // Устанавливаем роль на основе информации о сотруднике

                    // Выводим приветствие в зависимости от роли
                    string greetingMessage = $"Добро пожаловать, {employee.EmployeeSurname} {employee.EmployeeName}!";

                    if (currentUser.IsAdmin)
                    {
                        MessageBox.Show(greetingMessage + " (Администратор)");
                    }
                    else if (currentUser.IsMaster)
                    {
                        MessageBox.Show(greetingMessage + " (Мастер)");
                    }
                    else if (currentUser.IsEmployeeCall)
                    {
                        MessageBox.Show(greetingMessage + " (Сотрудник колл-центра)");
                    }
                    else if (currentUser.IsEmployeeDetails)
                    {
                        MessageBox.Show(greetingMessage + " (Сотрудник по деталям)");
                    }
                    else if (currentUser.IsEmployeeEmployee)
                    {
                        MessageBox.Show(greetingMessage + " (Сотрудник по деталям)");
                    }

                    // Открываем главное окно
                    WindowMain windowMain = new WindowMain(currentUser); // Передаем в новое окно
                    windowMain.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль.");
                    LoginTextBox.Clear();
                    PasswordTextBox.Clear();
                }
            }
        }
    }
}