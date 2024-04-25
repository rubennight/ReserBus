using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using ReserBus.ViewModel;

namespace ReserBus.View
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.VentaTickets_SelectViaje homePage = new View.VentaTickets_SelectViaje();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Main.Content = homePage;
        }
    }
}
