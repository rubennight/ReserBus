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
    /// Lógica de interacción para Config.xaml
    /// </summary>
    public partial class Config : Page
    {

        Boolean createRegistro;
        public Config()
        {
            InitializeComponent();
        }

        private void Toggle_Unidades_Form()
        {
            UnidadesForm.IsOpen = UnidadesForm.IsOpen ? false : true;
            Overlay1.Visibility = UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility = UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;

        }

        private void Toggle_Conductores_Form()
        {
            ChoferesForm.IsOpen = ChoferesForm.IsOpen ? false : true;
            Overlay1.Visibility = ChoferesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility = ChoferesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
        }

        private void Toggle_Datagrid(object sender, RoutedEventArgs e)
        {
            ConductoresDatagrid.Visibility = ConductoresDatagrid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            UnidadesDatagrid.Visibility = UnidadesDatagrid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void Reset_Forms()
        {
            txtApellidos.Text = "";
            txtCantidadAsientos.Text = "";
            txtModelo.Text = "";
            txtNombre.Text = "";
        }

        private void SetUnidadesForm(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                // Reemplazar el modelo
                //    if (dataGrid.SelectedItem is MODELO unidadSelected)
                //    {
                //        Toggle_Unidades_Form()
                //        txtModelo.Text = unidadSelected.modelo;
                //        txtCantidadAsientos.Text = unidadSelected.cantidadAsientos;
                //
                //    }
            }


        }

        private void SetConductoresForm(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                //// Reemplazar el modelo
                //if (dataGrid.SelectedItem is MODELO conductorSelected)
                //{
                //    Toggle_Conductores_Form()
                //    txtNombre.Text = conductorSelected.nombre;
                //    txtApellidos.Text = conductorSelected.apellidos;
                //
                //}
            }


        }

        private void CrearConductor(object sender, RoutedEventArgs e)
        {
            Toggle_Conductores_Form();
            Reset_Forms();

        }

        private void CrearUnidad(object sender, RoutedEventArgs e)
        {
            Toggle_Unidades_Form();
            Reset_Forms();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Toggle_Conductores_Form();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Toggle_Unidades_Form();
        }
    }
}
