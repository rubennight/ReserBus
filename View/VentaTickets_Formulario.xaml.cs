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

namespace ReserBus.View
{
    /// <summary>
    /// Lógica de interacción para VentaTickets_Formulario.xaml
    /// </summary>
    public partial class VentaTickets_Formulario : Page
    {
        public VentaTickets_Formulario()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.VentaTickets_SelectViaje selectViaje = new View.VentaTickets_SelectViaje();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Main.Content = selectViaje;
        }
    }
}
