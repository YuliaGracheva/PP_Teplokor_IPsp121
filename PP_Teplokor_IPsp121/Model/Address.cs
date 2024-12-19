using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Model
{
    public class Address
    {
        private string _city;
        private string _street;
        private string _home;
        private string _number;
        private string _comment;
        [Key]
        public int AddressID { get; set; }
        public string AddressCity
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("AddressCity");
            }
        }
        public string AddressStreet
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged("AddressStreet");
            }
        }
        public string AddressHome
        {
            get { return _home; }
            set
            {
                _home = value;
                OnPropertyChanged("AddressHome");
            }
        }
        public string AddressNumber
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged("AddressNumber");
            }
        }
        public string AddressComment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("AddressComment");
            }
        }
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public Address() { }
        public Address(int addressID, string addressCity, string addressStreet, string addressHome, string addressNumber, string addressComment, int clientID)
        {
            AddressID = addressID;
            AddressCity = addressCity;
            AddressStreet = addressStreet;
            AddressHome = addressHome;
            AddressNumber = addressNumber;
            AddressComment = addressComment;
            ClientID = clientID;
        }
        public string FullAddress => $"{AddressCity}, {AddressStreet}, {AddressHome}, {AddressNumber}";

        public Address ShallowCopy()
        {
            return (Address)this.MemberwiseClone();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
