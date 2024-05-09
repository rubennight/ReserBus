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

namespace ReserBus
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            View.Home homePage = new View.Home();
            View.VentaTickets_Formulario venta_form = new View.VentaTickets_Formulario();

            Main.Content = venta_form;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.Home homePage = new View.Home();
            Main.Content = homePage;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            View.VentaTickets_SelectViaje ventaTickets = new View.VentaTickets_SelectViaje();
            Main.Content = ventaTickets;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            View.Horarios horarios = new View.Horarios();
            Main.Content = horarios;
        }
    }
}
