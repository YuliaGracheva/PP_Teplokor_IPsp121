using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PP_Teplokor_IPsp121.Helper
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public Action CreateReport { get; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action createReport)
        {
            CreateReport = createReport;
        }

        public bool CanExecute(object parametr)
        {
            return this.canExecute == null || this.canExecute(parametr);
        }
        public void Execute(object parametr)
        {
            this.execute(parametr);
        }
    }
}