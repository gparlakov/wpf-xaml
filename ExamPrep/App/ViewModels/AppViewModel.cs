using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Applicate.Behavior;
using Applicate.Data;
using Applicate.Helpers;

namespace Applicate.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private ICommand changeViewModelCommand;
        private IPageViewModel currentViewModel;
        private bool loggedInUser = false;
        private ICommand logoutCommand;
        private string username;

        public AppViewModel()
        {
            this.ViewModels = new List<IPageViewModel>();
            this.ViewModels.Add(new TodoListsViewModel());
            //this.ViewModels.Add(new AppointmentsViewModel());
            var loginVM = new LoginRegisterFormViewModel();

            loginVM.LoginSuccess += this.LoginSuccessful;
            this.LoginRegisterVM = loginVM;
            this.CurrentViewModel = this.LoginRegisterVM;
        }

        public bool LoggedInUser
        {
            get
            {
                return this.loggedInUser;
            }
            set
            {
                this.loggedInUser = value;
                this.OnPropertyChanged("LoggedInUser");
            }
        }

        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
                OnPropertyChanged("Username");
            }
        }

        public IPageViewModel CurrentViewModel
        {
            get
            {
                return this.currentViewModel;
            }
            set
            {
                this.currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        public LoginRegisterFormViewModel LoginRegisterVM { get; set; }

        public List<IPageViewModel> ViewModels { get; set; }

        public ICommand ChangeViewModel
        {
            get
            {
                if (this.changeViewModelCommand == null)
                {
                    this.changeViewModelCommand =
                        new RelayCommand(this.HandleChangeViewModelCommand);
                }
                return this.changeViewModelCommand;
            }
        }

        public ICommand Logout
        {
            get
            {
                if (this.logoutCommand == null)
                {
                    this.logoutCommand = new RelayCommand(this.HandleLogoutCommand);
                }
                return this.logoutCommand;
            }
        }

        private void HandleLogoutCommand(object parameter)
        {
            bool isUserLoggedOut = DataPersister.LogoutUser();
            if (isUserLoggedOut)
            {
                this.Username = "";
                this.LoggedInUser = false;
                //this.CurrentViewModel = this.LoginRegisterVM;
                this.HandleChangeViewModelCommand(this.LoginRegisterVM);
            }
        }

        private void HandleChangeViewModelCommand(object parameter)
        {
            var newCurrentViewModel = parameter as IPageViewModel;
            this.CurrentViewModel = newCurrentViewModel;
        }

        public void LoginSuccessful(object sender, LoginSuccessArgs e)
        {
            this.Username = e.Username;
            this.LoggedInUser = true;
            this.HandleChangeViewModelCommand(this.ViewModels[0]);
        }
    }
}