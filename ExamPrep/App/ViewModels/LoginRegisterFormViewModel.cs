using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Applicate.Behavior;
using Applicate.Data;
using Applicate.Helpers;
using System.IO;

namespace Applicate.ViewModels
{
    public class LoginRegisterFormViewModel : ViewModelBase, IPageViewModel
    {
        private string message;
        private ICommand loginCommand;
        private ICommand registerCommand;
        
        public string Name
        {
            get
            {
                return "Login Form";
            }
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        public ICommand Login
        {
            get
            {
                if (this.loginCommand == null)
                {
                    this.loginCommand = new RelayCommand(this.HandleLoginCommand);
                }
                return this.loginCommand;
            }
        }

        public ICommand Register
        {
            get
            {
                if (this.registerCommand == null)
                {
                    this.registerCommand = new RelayCommand(this.HandleRegisterCommand);
                }
                return this.registerCommand;
            }
        }

        public event EventHandler<LoginSuccessArgs> LoginSuccess;

        public LoginRegisterFormViewModel()
        {
            this.Username = "parlakov";
            //this.Email = "parlakov@abv.bg.com";
        }
        
        private void HandleRegisterCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;
         
            var authenticationCode = this.GetSHA1HashData(password);

            try
            {
                DataPersister.RegisterUser(this.Username, this.Email, authenticationCode);
                this.HandleLoginCommand(parameter);
            }
            catch(ArgumentException argEx)
            {
                this.Message = argEx.Message;
            }
            catch (Exception ex)
            {                
                this.Message = ex.Message;
            }
        }

        private void HandleLoginCommand(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;
            var authenticationCode = this.GetSHA1HashData(password);
            try
            {
                var username = DataPersister.LoginUser(this.Username, authenticationCode);

                if (!string.IsNullOrEmpty(username))
                {
                    this.RaiseLoginSuccess(username);
                }
            }
            catch (ArgumentException argEx)
            {
                this.Message = argEx.Message;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
        }

        private string GetSHA1HashData(string data)
        {
            var dataBytes = Encoding.Default.GetBytes(data);
            //return "0123456789012345678901234567890123456789";
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] hash = sha1.ComputeHash(dataBytes);
            string delimitedHexHash = BitConverter.ToString(hash);
            string hexHash = delimitedHexHash.Replace("-", "");
            return hexHash;

            //SHA1 sha = new SHA1CryptoServiceProvider();
            //var passwordBytes = Encoding.Default.GetBytes(password);
            //passwordBytes = Encoding.Convert(Encoding.Default,Encoding.UTF8,passwordBytes);
            //var authenticationCodeBytes = sha.ComputeHash(passwordBytes);
            //var authenticationCode = Encoding.UTF8.GetString(authenticationCodeBytes);
        }

        protected void RaiseLoginSuccess(string username)
        {
            if (this.LoginSuccess!= null)
            {
                this.LoginSuccess(this, new LoginSuccessArgs(username));
            }
        }
    }
}