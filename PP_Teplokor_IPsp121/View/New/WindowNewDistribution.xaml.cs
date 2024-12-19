using PP_Teplokor_IPsp121.Helper;
using PP_Teplokor_IPsp121.Model;
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

namespace PP_Teplokor_IPsp121.View.New
{
    public partial class WindowNewDistribution : Window
    {
        public WindowNewDistribution()
        {
            InitializeComponent();
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Distribution distribution)
            {
                distribution.EmployeeID = distribution.SelectedEmployee.EmployeeID;
                MyDbContext dbContext = new MyDbContext();
                dbContext.SaveEntity<Distribution>(distribution); // Сохраняем распределение
                this.DialogResult = true;
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
