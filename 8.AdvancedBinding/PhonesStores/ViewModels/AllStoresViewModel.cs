using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PhonesStore.Commands;

namespace PhonesStore.ViewModels
{
    public class AllStoresViewModel : ViewModelBase
    {
        private ObservableCollection<PhonesStoreViewModel> allStores;
        private PhonesStoreViewModel newStore;
        private ICommand addNewStore;
        private PhonesStoreViewModel selectedStore;
        private PhoneViewModel selectedPhone;
        private ObservableCollection<PhoneViewModel> allPhones;
        private ICommand addPhoneToStore;


        // constructor
        public AllStoresViewModel()
        {
            this.PhonesStoreDocumentPath = "..\\..\\xml-data\\";
            this.NewStore = new PhonesStoreViewModel();
        }

        public string PhonesStoreDocumentPath { get; set; }

        public PhonesStoreViewModel SelectedStore
        {
            get
            {
                return this.selectedStore;
            }
            set
            {
                this.selectedStore = value;
                OnPropertyChanged("SelectedStore");
            }
        }

        public PhoneViewModel SelectedPhone
        {
            get
            {
                return this.selectedPhone;
            }
            set
            {
                this.selectedPhone = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public IEnumerable<PhonesStoreViewModel> AllStores
        {
            get
            {
                if (this.allStores == null) 
                {
                    this.AllStores = DataPersister.GetAllStores(this.PhonesStoreDocumentPath);
                }
                return this.allStores;
            }
            set
            {
                if (this.allStores == null)
                {
                    this.allStores = new ObservableCollection<PhonesStoreViewModel>();
                }
                this.allStores.Clear();
                foreach (var item in value)
                {
                    this.allStores.Add(item);
                }
            }
        }

        public PhonesStoreViewModel NewStore
        {
            get
            {
                return this.newStore;
            }
            set
            {
                this.newStore = value;
                OnPropertyChanged("NewStore");
            }
        }

        public ICommand AddNewStore
        {
            get
            {
                if (this.addNewStore == null)
                {
                    this.addNewStore = new RelayCommand(
                        (obj) =>
                        {
                            DataPersister.AddNewStore(this.NewStore, this.PhonesStoreDocumentPath);
                            this.NewStore = new PhonesStoreViewModel();
                            this.AllStores = DataPersister.GetAllStores(this.PhonesStoreDocumentPath);
                        });
                }
                return this.addNewStore;
            }
        }

        public ICommand AddPhoneToStore 
        {
            get 
            {
                if (this.addPhoneToStore == null)
                {
                    this.addPhoneToStore = new RelayCommand(
                        (obj) =>
                        {
                            DataPersister.SavePhoneToStore(this.PhonesStoreDocumentPath, this.SelectedStore.Name, this.SelectedPhone.Id);
                            this.SelectedStore.Phones = DataPersister.GetPhonesByStoreName(this.PhonesStoreDocumentPath, this.SelectedStore.Name);
                        });
                }
                return this.addPhoneToStore;
            }
        }

        public IEnumerable<PhoneViewModel> AllPhones
        {
            get
            {
                if (this.allPhones == null)
                {
                    this.AllPhones = DataPersister.GetPhones(this.PhonesStoreDocumentPath);
                }
                return this.allPhones;
            }
            set
            {
                if (this.allPhones == null)
                {
                    this.allPhones = new ObservableCollection<PhoneViewModel>();
                }
                this.allPhones.Clear();
                foreach (var item in value)
                {
                    this.allPhones.Add(item);
                }
            }
            
        }
    }
}
