using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasksManager.WpfClient.ViewModels
{
   public class TodoListViewModel
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public IQueryable<TodoViewModel> Todos { get; set; }
    }
}
