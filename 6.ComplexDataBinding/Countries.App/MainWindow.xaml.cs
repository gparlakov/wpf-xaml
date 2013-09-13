using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Countries.Data;

namespace Countries.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Data.CountriesViewModel copy;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new Data.CountriesViewModel();
            this.copy = new Data.CountriesViewModel();

        }

        private void Countries_SaveChangesButton(object sender, RoutedEventArgs e)
        {
            var dataContext = (Data.CountriesViewModel)this.CountriesGrid.DataContext;
            foreach (var country in dataContext.Countries)
            {
                SaveCountryIfChanged(country);
            }

        }

        private void Towns_SaveChangesButton(object sender, RoutedEventArgs e)
        {
            var dataContext = (Data.CountriesViewModel)this.TownsGrid.DataContext;
            foreach (var town in dataContext.Towns)
            {
                SaveTownIfChanged(town);
            }

        }

        private void SaveTownIfChanged(Town town)
        {
            var cachedCopy = this.copy.Towns.FirstOrDefault(t => t.Id == town.Id);
            if (cachedCopy == null)
            {
                using (var context = new Data.WorldEntities())
                {
                    context.Towns.Add(new Town
                    {
                        Name = town.Name
                    });
                }
            }

            if (cachedCopy.Name != town.Name)
            {
                using (var context = new Data.WorldEntities())
                {
                    var townInDb = context.Towns.Find(town.Id);
                    townInDb.Name = town.Name;
                    context.SaveChanges();
                }
            }
        }

        private void SaveCountryIfChanged(Country country)
        {
            var cachedCopy = this.copy.Countries.FirstOrDefault(c => c.Id == country.Id);
            if (cachedCopy == null)
            {
                using (var context = new Data.WorldEntities())
                {
                    context.Countries.Add(new Country 
                    { 
                        Name = country.Name, 
                        Population = country.Population 
                    });
                }
            }

            if (cachedCopy.Name != country.Name || cachedCopy.Population != country.Population)
            {
                using (var context = new Data.WorldEntities())
                {
                    var countryInDb = context.Countries.Find(country.Id);
                    countryInDb.Name = country.Name;
                    countryInDb.Population = country.Population;
                    context.SaveChanges();
                }
            }
        }
    }
}
