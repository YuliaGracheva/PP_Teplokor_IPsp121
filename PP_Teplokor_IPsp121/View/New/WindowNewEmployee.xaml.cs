using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PP_Teplokor_IPsp121.View.New
{
    public partial class WindowNewEmployee : Window
    {
        public WindowNewEmployee()
        {
            InitializeComponent();
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            string surname = txtSurname.Text;
            string name = txtName.Text;
            string patronymic = txtPatronymic.Text;
            string login = txtLogin.Text; // Предполагается, что у вас есть поле с именем txtLogin
            string password = txtPassword.Text;
            if (!IsValidName(surname))
            {
                MessageBox.Show("Фамилия может содержать только буквы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Не устанавливаем DialogResult
            }

            if (!IsValidName(name))
            {
                MessageBox.Show("Имя может содержать только буквы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Не устанавливаем DialogResult
            }

            if (!IsValidName(patronymic))
            {
                MessageBox.Show("Отчество может содержать только буквы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Не устанавливаем DialogResult
            }
            // Проверка логина
            if (!IsValidLogin(login))
            {
                MessageBox.Show("Логин должен содержать не менее 6 символов и включать специальные символы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Не устанавливаем DialogResult
            }

            // Проверка пароля
            if (!IsValidPassword(password))
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов и включать специальные символы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Не устанавливаем DialogResult
            }
            this.DialogResult = true;
            this.Close(); // Закрываем диалоговое окно
        }
        private bool IsValidLogin(string login)
        {
            return !string.IsNullOrWhiteSpace(login) && login.Length >= 6 && login.Any(ch => !char.IsLetterOrDigit(ch));
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6 && password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        private bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
        }
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
