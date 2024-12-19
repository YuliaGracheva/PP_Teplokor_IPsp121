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
    public partial class WindowNewAddress : Window
    {
        public WindowNewAddress()
        {
            InitializeComponent();
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {

            string surname = txtSurname.Text;
            if (!IsValidName(surname))
            {
                MessageBox.Show("Название города может содержать только буквы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Не устанавливаем DialogResult
            }

            DialogResult = true;
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
