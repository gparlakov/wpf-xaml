using System;
using System.Linq;
using PhonesStore.ViewModels;

namespace PhonesStore.ViewModels
{
    public class PhoneViewModel
    {
        public string Id { get; set; }
        public string Model { get; set; }
        
        public OperatingSystemViewModel OS { get; set; }

        public string Vendor { get; set; }

        public int Year { get; set; }

        public string ImagePath { get; set; }
    }
}