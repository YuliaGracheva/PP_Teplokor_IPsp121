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
    public class Details : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private int _count;
        private decimal _price;
        [Key]
        public int DetailsID { get; set; }
        public string DetailsName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("DetailsName");
            }
        }
        public string DetailsDescription
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("DetailsDescription");
            }
        }
        public int DetailsCount
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("DetailsCount");
            }
        }
        public decimal DetailsPrice
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("DetailsPrice");
            }
        }

        public Details() { }

        public Details(int detailsID, string detailsName, string detailsDescription, int detailsCount, decimal detailsPrice)
        {
            DetailsID = detailsID;
            DetailsName = detailsName;
            DetailsDescription = detailsDescription;
            DetailsCount = detailsCount;
            DetailsPrice = detailsPrice;
        }

        public Details ShallowCopy()
        {
            return (Details)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
