using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
using PP_Teplokor_IPsp121.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PP_Teplokor_IPsp121.View
{
    public partial class WindowCategoryError : Window
    {
        public ObservableCollection<string> MenuItems { get; set; }
        public WindowCategoryError(CurrentUser currentUsers)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            DataContext = new CategoryErrorViewModel(currentUsers);
            currentUser = currentUsers;
            
        }
        private CurrentUser currentUser;
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный элемент как строку
            var selectedItem = (string)((ListBox)sender).SelectedItem;

            if (selectedItem != null)
            {
                this.Hide();

                switch (selectedItem)
                {
                    case "На главную":
                        var mainWindow = new WindowMain(currentUser);
                        mainWindow.Show();
                        break;
                    case "Заявки":
                        var zayavkiWindow = new WindowApplicationDiagnostics(currentUser);
                        zayavkiWindow.Show();
                        break;
                    case "Сотрудники":
                        var employeesWindow = new WindowEmployee(currentUser);
                        employeesWindow.Show();
                        break;
                    case "Детали":
                        var detailsWindow = new WindowDetails(currentUser);
                        detailsWindow.Show();
                        break;
                    case "Категории ошибок":
                        var errorCategoriesWindow = new WindowCategoryError(currentUser);
                        errorCategoriesWindow.Show();
                        break;
                    case "Распределение":
                        var distributionWindow = new WindowDistribution(currentUser);
                        distributionWindow.Show();
                        break;
                    case "Выйти из аккаунта":
                        var windowMain = new MainWindow();
                        currentUser = null;
                        windowMain.Show();
                        break;
                    case "Выйти из приложения":
                        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            Application.Current.Shutdown();
                        }
                        break;
                }
            }
        }
    }
}
