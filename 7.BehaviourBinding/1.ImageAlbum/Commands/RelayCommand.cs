using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageAlbum.Commands
{
    public delegate bool CanExecuteDelegate(object obj);
    public delegate void ExecuteDelegate(object obj);
    public class RelayCommand : ICommand
    {
        private ExecuteDelegate executeDelegate;

        private CanExecuteDelegate canExecuteDelegate;

        public RelayCommand(ExecuteDelegate executeDelegate,
            CanExecuteDelegate canExecuteDelegate)
        {
            this.executeDelegate = executeDelegate;
            this.canExecuteDelegate = canExecuteDelegate;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecuteDelegate(parameter);
        }

        public void Execute(object parameter)
        {
            executeDelegate(parameter);
        }
    }
}
