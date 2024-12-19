using PP_Teplokor_IPsp121.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class WindowNewClient : Window
    {
        public WindowNewClient()
        {
            InitializeComponent();
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            string surname = txtSurname.Text;
            string name = txtName.Text;
            string patronymic = txtPatronymic.Text;
            string phoneNumber = txtPhoneNumber.Text;

            // Проверка фамилии, имени и отчества
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

                // Проверка номера телефона
                if (!IsValidPhoneNumber(phoneNumber))
                {
                    MessageBox.Show("Номер телефона должен начинаться с 7 или 8 и содержать 11 цифр.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Не устанавливаем DialogResult
                }
                // Устанавливаем DialogResult в true
                this.DialogResult = true;
                this.Close(); // Закрываем диалоговое окно
        }
        private bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^8d{10}$|^7d{10}$");
        }

        private void SaveClientData(ClientViewModel viewModel)
        {
            // Логика сохранения данных
            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
