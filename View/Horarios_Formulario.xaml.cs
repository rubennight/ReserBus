using ReserBus.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        SqlConnection conexionSql;
        public Horarios_Formulario()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);
            llenaOrigen();
            llenaDestino();
            llenaSucursal();
        }

        private void llenaOrigen() 
        {
            try
            {
                string consulta = "select * from [dbo v_1.3].destinos";
                SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
                DataTable tablaResultado = new DataTable();
                adaptadorSql.Fill(tablaResultado);
                CBOrigen.DisplayMemberPath = "ciudad";
                CBOrigen.SelectedValuePath = "id_destino";
                CBOrigen.ItemsSource = tablaResultado.DefaultView;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Ocurrió un error al ejecutar la consulta SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
        }

        private void llenaDestino()
        {
            string consulta = "SELECT * FROM [dbo v_1.3].destinos";
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
            DataTable tablaResultado = new DataTable();
            adaptadorSql.Fill(tablaResultado);
            CBDestino.DisplayMemberPath = "ciudad";
            CBDestino.SelectedValuePath = "id_destino";
            CBDestino.ItemsSource = tablaResultado.DefaultView;
        }

        private void llenaSucursal()
        {
            string consulta = "SELECT * FROM [dbo v_1.3].destinos";
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
            DataTable tablaResultado = new DataTable();
            adaptadorSql.Fill(tablaResultado);
            CBSucursal.ItemsSource = tablaResultado.DefaultView;
            CBSucursal.SelectedValuePath = "id_destino";
            CBSucursal.DisplayMemberPath = "ciudad";
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
