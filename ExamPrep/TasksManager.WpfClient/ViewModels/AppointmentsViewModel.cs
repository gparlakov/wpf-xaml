using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasksManager.App.Models;

namespace TasksManager.WpfClient.ViewModels
{
    public class AppointmentsViewModel : ViewModelBase,IPageViewModel
    {
        private ObservableCollection<AppointmentViewModel> appointments;

        private ICommand addNewAppointment;

        private string message;

        public AppointmentsViewModel()
        {
            this.AppointmentDate = DateTime.Now;
        }

        public ICommand AddNewAppointment
        {
            get
            {
                if (this.addNewAppointment == null)
                {
                    this.addNewAppointment = 
                        new Behavior.RelayCommand(this.HandleAddNewAppointment);
                }
                return this.addNewAppointment;
            }
        }

        private void HandleAddNewAppointment(object parameter)
        {
            try
            {
                var response = Data.DataPersister.AddNewAppointment(
                    this.Subject, this.Description, this.AppointmentDate, this.Duration);
                this.Message = "Created !";
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
        }

        public IEnumerable<AppointmentViewModel> Appointments
        {
            get
            {
                if (this.appointments == null)
                {
                    this.Appointments = Data.DataPersister.GetAllAppointments();
                }
                return this.appointments;
            }
            set
            {
                if (this.appointments == null)
                {
                    this.appointments = new ObservableCollection<AppointmentViewModel>();
                }
                this.appointments.Clear();
                foreach (var item in value)
                {
                    this.appointments.Add(item);
                }
            }
        }

        public string Name
        {
            get { return "Appointements"; }
        }

        public string Subject { get; set; }

        public string Description { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int Duration { get; set; }

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

    }
}
