using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasksManager.WpfClient.Behavior;
using TasksManager.WpfClient.Data;

namespace TasksManager.WpfClient.ViewModels
{
    public class TodoListsViewModel : ViewModelBase, IPageViewModel
    {
        private ICommand createCommand;

        private ICommand addTodoCommand;

        private string title;

        private string message;

        public string Message
        {
            get 
            { 
                return this.message; 
            }
            set 
            { 
               this.message = value;
               OnPropertyChanged("Message");
            }
        }
        
        private ObservableCollection<TodoListViewModel> todoLists;

        public string Name
        {
            get
            {
                return "Todolists View";
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        public ObservableCollection<TodoViewModel> Todos { get; set; }

        public IEnumerable<TodoListViewModel> TodoLists
        {
            get
            {
                if (this.todoLists == null)
                {
                    this.TodoLists = DataPersister.GetTodoLists();
                }
                return this.todoLists;
            }
            set
            {
                if (this.todoLists == null)
                {
                    this.todoLists = new ObservableCollection<TodoListViewModel>();
                }
                this.todoLists.Clear();
                foreach (var item in value)
                {
                    this.todoLists.Add(item);
                }
            }
        }

        public ICommand Create
        {
            get
            {
                if (this.createCommand == null)
                {
                    this.createCommand = new RelayCommand(this.HandleCreateListCommand);
                }
                return this.createCommand;
            }
        }

        public ICommand AddTodo
        {
            get
            {
                if (this.addTodoCommand == null)
                {
                    this.addTodoCommand = new RelayCommand(this.HandleAddTodoCommand);
                }
                return this.addTodoCommand;
            }
        }
        
        private void HandleAddTodoCommand(object parameter)
        {
            this.Todos.Add(new TodoViewModel());
        }

        private void HandleCreateListCommand(object parameter)
        {
            if (this.Title == null)
            {
                this.Message = "Title can not be null!";
                return;
            }

            if (this.Todos.Count == 0)
	        {
                this.Message = "Todo list must contain at least one !";
                return;
	        }
            var filledTodoes =  this.Todos.Where(t => !string.IsNullOrEmpty(t.Text));

            if (filledTodoes.Count() == 0)
            {
                this.Message = "Todo list must contain at least one Todo!";
                return;
            }

            DataPersister.CreateNewTodosList(this.Title, filledTodoes);
            this.Title = "";
            this.Todos.Clear();
            this.TodoLists = DataPersister.GetTodoLists();
        }

        public TodoListsViewModel()
        {
            this.Todos = new ObservableCollection<TodoViewModel>();
            this.Todos.Add(new TodoViewModel()
            {
                Text = "sample"
            });
        }
        /*
        {"Title":"Domakinska Rabota",
        "Todos": [{"text": "chiniite"},{"text": "praha"}, {"text":"peralnq"},"{text": "metlata"}]
        }
        */
    }
}