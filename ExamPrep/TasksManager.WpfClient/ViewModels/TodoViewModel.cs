using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TasksManager.WpfClient.Behavior;
using TasksManager.WpfClient.Data;

namespace TasksManager.WpfClient.ViewModels
{
    public class TodoViewModel:ViewModelBase
    {
        private string text;
        private ICommand changeStateCommand;
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        public ICommand ChangeState
        {
            get
            {
                if (this.changeStateCommand == null)
                {
                    this.changeStateCommand = new RelayCommand(this.HandleChangeStateCommand);
                }
                return this.changeStateCommand;
            }
        }

        private void HandleChangeStateCommand(object parameter)
        {
            DataPersister.ChangeState(this.Id);
        }

        public int Id { get; set; }

        public bool IsDone { get; set; }
    }
}
