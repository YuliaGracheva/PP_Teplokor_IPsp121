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
    public class CategoryError : INotifyPropertyChanged
    {
        private string _name;
        private string _description;

        [Key]
        public int CategoryErrorID { get; set; }
        public string CategoryErrorName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("CategoryErrorName");
            }
        }

        public string CategoryErrorDescriprion
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("CategoryErrorDescriprion");
            }
        }
        public CategoryError() { }
        public CategoryError(int categoryErrorID, string categoryErrorName,  string categoryErrorDescriprion)
        {
            CategoryErrorID = categoryErrorID;
            CategoryErrorName = categoryErrorName;
            CategoryErrorDescriprion = categoryErrorDescriprion;
        }

        public CategoryError ShallowCopy()
        {
            return (CategoryError)this.MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
