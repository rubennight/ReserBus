using System;
using System.Collections.Generic;
using System.Data;
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
        DataRowView selectedItem;
        public VentaTickets_Formulario(DataRowView selectedItem)
        {
            InitializeComponent();

            txtDestino.Text = selectedItem["destino"].ToString();
            txtOrigen.Text = selectedItem["origen"].ToString();
            txtFechaSalida.Text = selectedItem["fecha_hora_salida"].ToString();
            txtCosto.Text = "$ " + selectedItem["precio"].ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.VentaTickets_SelectViaje selectViaje = new View.VentaTickets_SelectViaje();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Main.Content = selectViaje;
        }
    }
}
