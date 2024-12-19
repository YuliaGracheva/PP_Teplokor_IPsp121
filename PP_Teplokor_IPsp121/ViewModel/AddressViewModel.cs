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
using System.Windows;
using PP_Teplokor_IPsp121.View.New;

namespace PP_Teplokor_IPsp121.ViewModel
{
    public class AddressViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Address> ListAddress { get; set; }
        public static int AddressID;
        public int MaxId()
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

        public AddressViewModel(CurrentUser currentUser)
        {
            string connectionString = "Data Source=C:\\Users\\user\\Desktop\\PP_Teplokor_IPsp121\\PP_Teplokor_IPsp121\\BD\\PP_TeploKor.db";

            List<Address> employee = MyDbContext.GetEntities<Address>(connectionString, @"
    SELECT * FROM Address cl 
    LEFT JOIN Archive a ON a.AddressID = cl.AddressID WHERE a.ArchiveID IS NULL;");


            // Преобразование в ObservableCollection
            ListAddress = new ObservableCollection<Address>(employee);
        }

        private RelayCommand editAddress;
        public RelayCommand EditAddress
        {
            get
            {
                return editAddress ??
                (editAddress = new RelayCommand(obj =>
                {
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
                     WindowNewAddress wnEmployee = new WindowNewAddress
                     {
                         Title = "Новый сотрудник",
                     };
                     int maxIdAddress = MaxId() + 1;
                     Address employee = new Address { AddressID = maxIdAddress };
                     wnEmployee.DataContext = employee;
                     if (wnEmployee.ShowDialog() == true)
                     {
                         ListAddress.Add(employee);
                         MyDbContext dbContext = new MyDbContext();
                         dbContext.SaveEntity<Address>(employee);
                     }
                     SelectedAddress = employee;
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
                }, (obj) => SelectedAddress != null && ListAddress.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
