using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicate.ViewModels
{
    public class TodoListsViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get { return "Todo list "; }
        }

    }
}
