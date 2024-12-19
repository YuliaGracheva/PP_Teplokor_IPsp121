using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Model
{
    public class Client : INotifyPropertyChanged
    {
        private string _surname;
        private string _name;
        private string _patronymic;
        private string _number;

        [Key]
        public int ClientID { get; set; }
        public string ClientSurname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("ClientSurname");
            }
        }
        public string ClientName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("ClientName");
            }
        }
        public string ClientPatronymic
        {
            get { return _patronymic; }
            set
            {
                _patronymic = value;
                OnPropertyChanged("ClientPatronymic");
            }
        }
        public string ClientNumber
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged("ClientNumber");
            }
        }

        public Client() { }
        public Client(int clientID, string clientSurname, string clientName, string clientPatronymic, string clientNumber)
        {
            ClientID = clientID;
            ClientSurname = clientSurname;
            ClientName = clientName;
            ClientPatronymic = clientPatronymic;
            ClientNumber = clientNumber;
        }

        public Client ShallowCopy()
        {
            return (Client)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}