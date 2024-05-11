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
using Xceed.Wpf.Toolkit;

namespace ReserBus.View
{
    /// <summary>
    /// Lógica de interacción para Horarios_Formulario.xaml
    /// </summary>
    public partial class Horarios_Formulario : Page
    {
        private List<DataRowView> ciudadesSeleccionadas = new List<DataRowView>();
        //Variables para la consulta insert.
        string unidad;
        string chofer;
        string fechaHoraSalida;
        string fechaHoraLlegada;
        string consulta;

        

        SqlConnection conexionSql;
        public Horarios_Formulario()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);
            Console.WriteLine(conexion);
            llenaDestino();
            llenaSucursal();
            llenaChofer();
            llenaUnidad();
        }



        private void llenaDestino()
        {


            try
            {
                string consulta = "select d.id_destino,d.estado, d.ciudad, d.precio_base from [dbo v_1.3].destinos d";
                SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
                DataTable tablaResultado = new DataTable();
                adaptadorSql.Fill(tablaResultado);
                DGDestinos.SelectedValuePath = "id_destino";
                DGDestinos.ItemsSource = tablaResultado.DefaultView;
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

        private void llenaSucursal()
        {
            string consulta = "SELECT * FROM [dbo v_1.3].destinos";
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
            DataTable tablaResultado = new DataTable();
            adaptadorSql.Fill(tablaResultado);
            Sucursal.ItemsSource = tablaResultado.DefaultView;
            Sucursal.SelectedValuePath = "id_destino";
            Sucursal.DisplayMemberPath = "ciudad";
        }

        private void llenaChofer()
        {
            try
            {
                string consulta = "select c.id_chofer, (c.nombre+', '+ c.apellidos) as nombre_completo from [dbo v_1.3].choferes c";
                SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
                DataTable tablaResultado = new DataTable();
                adaptadorSql.Fill(tablaResultado);
                CBConductor.ItemsSource = tablaResultado.DefaultView;
                CBConductor.SelectedValuePath = "id_chofer";
                CBConductor.DisplayMemberPath = "nombre_completo";
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

        private void llenaUnidad()
        {
            string consulta = "SELECT * FROM [dbo v_1.3].unidades";
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
            DataTable tablaResultado = new DataTable();
            adaptadorSql.Fill(tablaResultado);
            CBUnidad.ItemsSource = tablaResultado.DefaultView;
            CBUnidad.SelectedValuePath = "id_unidad";
            CBUnidad.DisplayMemberPath = "modelo";
        }

        private void insertaNuevoViajeYRuta(object sender, RoutedEventArgs e)
        {
            consulta = "INSERT INTO [dbo v_1.3].viajes_programados\r\n" +
                "(id_viaje_programado,id_unidad, id_chofer, fecha_hora_salida,fecha_hora_llegada_estimada,cupo)\r\n" +
                "VALUES\r\n" +
                "(NEWID(),@unidad,@chofer,@fechaHoraSalida,@fechaHoraLlegada,1)";
            SqlCommand comandoSql = new SqlCommand(consulta, conexionSql);
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(comandoSql);
            DataTable tablaResultado = new DataTable();
            comandoSql.Parameters.AddWithValue("unidad",unidad);
            comandoSql.Parameters.AddWithValue("chofer",chofer);
            comandoSql.Parameters.AddWithValue("fechaHoraSalida", fechaHoraSalida);
            comandoSql.Parameters.AddWithValue("fechaHoraLlegada", fechaHoraLlegada);
            adaptadorSql.Fill(tablaResultado);
            
        }


        private void DoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                // Reemplazar el modelo
                var selectedItem = dataGrid.SelectedItem;
                DataRowView rowView = (DataRowView)dataGrid.SelectedItem;


                ciudadesSeleccionadas.Add(rowView);

                // Proyectar solo los valores de "ciudad" y unirlos en una cadena
                var ciudades = ciudadesSeleccionadas.Select(row => row["ciudad"].ToString());

                txtDestinosSeleccionados.Text = string.Join(" - ", ciudades);


            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ciudadesSeleccionadas.Clear();
            txtDestinosSeleccionados.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.Horarios horarios = new View.Horarios();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        }

            mainWindow.Main.Content = horarios;
        //Obtenemos el conductor seleccionado.
        private void CBConductor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chofer = CBConductor.SelectedValue.ToString();
        }

        //Obtenemos la fecha seleccionada.
        private void DateTimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string fecha = "";
            string fechaLlegada = "";
            DateTime? fechaNullable = DTPFechaHoraSalida.Value;
            if (fechaNullable.HasValue)
            {
                DateTime fecha1 = fechaNullable.Value;
                DateTime fecha2 = fecha1.AddDays(1);
                fecha = fecha1.ToString("yyyy/MM/dd HH:mm");
                fechaLlegada = fecha2.ToString("yyyy/MM/dd HH:mm");
            }

            Console.WriteLine(fecha +"     " +  fechaLlegada);

        }

        //Obtenemos la unidad seleccionada.
        private void CBUnidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            unidad = CBUnidad.SelectedValue.ToString();
        }
    }
}
