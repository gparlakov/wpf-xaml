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
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new Data.CountriesViewModel();
        }

        private void Countries_SaveChangesButton(object sender, RoutedEventArgs e)
        { 
            using (var context = new Data.WorldEntities())
            {
               var x = e.Source;
            }
        }
    }
}
