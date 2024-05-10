using ReserBus.Model;
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
    /// Lógica de interacción para Horarios_Formulario.xaml
    /// </summary>
    public partial class Horarios_Formulario : Page
    {
        private List<string> ciudadesSeleccionadas = new List<string>();

        public Horarios_Formulario()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            //if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            //{
            //    // Reemplazar el modelo
            //    if (dataGrid.SelectedItem is MODELO destinoSelected)
            //    {
            //        // Almacenar la ciudad del elemento seleccionado en el array
            //        ciudadesSeleccionadas.Add(destinoSelected.Ciudad);
            //
            //        // Actualizar el texto de txtDestinosSeleccionados con las ciudades unidas mediante " - "
            //        txtDestinosSeleccionados.Text = string.Join(" - ", ciudadesSeleccionadas);
            //    }
            //}


        }
    }
}
