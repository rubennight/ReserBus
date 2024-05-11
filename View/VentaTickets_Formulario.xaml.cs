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
    /// Lógica de interacción para VentaTickets_Formulario.xaml
    /// </summary>
    public partial class VentaTickets_Formulario : Page
    {
        DataRowView selectedItem;

        SqlConnection miConexionSql;

        public VentaTickets_Formulario(DataRowView selectedItem)
        {
            InitializeComponent();

            DataContext = this;

            string miConexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);
        }

        public void InsertarVentaTicket( Decimal precioTotal, Int32 numAsiento, string nombre, string apellidos, string telefono)
        {
            //primero generamos un id para el boleto, id para el pasajero y luego la fecha actual:
            Guid idBoleto = Guid.NewGuid();
            Guid idPasajero = Guid.NewGuid();
            DateTime fechaVenta = DateTime.Now;

            string destino = selectedItem["destino"].ToString();
            string origen = selectedItem["origen"].ToString();
            DateTime fechaSalida = Convert.ToDateTime(selectedItem["fecha_hora_salida"]);
            Decimal costo =  Convert.ToDecimal(selectedItem["precio"].ToString());
            try
            {
                miConexionSql.Open();
                //primero insertamos al pasajero 
                string queryInsertPasajero = "INSERT INTO pasajeros (id_pasajero, nombre, apellidos, telefono) VALUES (@IdPasajero, @Nombre, @Apellidos, @Telefono)"; ;
                SqlCommand commandInsertPasajero = new SqlCommand(queryInsertPasajero, miConexionSql);

                commandInsertPasajero.Parameters.AddWithValue("@IdPasajero", idPasajero);
                commandInsertPasajero.Parameters.AddWithValue("@Nombre", nombre);
                commandInsertPasajero.Parameters.AddWithValue("@Apellidos", apellidos);
                commandInsertPasajero.Parameters.AddWithValue("@Telefono", telefono);

                commandInsertPasajero.ExecuteNonQuery();

                string queryInsertBoleto = "INSERT INTO boletos (id_boleto, id_pasajero, id_ruta, destino_origen_nombre, destino_final_nombre, fecha_venta, precio_total) \r\n VALUES (@IdBoleto, @IdPasajero, @IdRuta, @DestinoOrigen, @DestinoFinal, @FechaVenta, @PrecioTotal)";

                SqlCommand commandInsertBoleto = new SqlCommand(queryInsertBoleto, miConexionSql);

                commandInsertBoleto.Parameters.AddWithValue("@IdBoleto", idBoleto);
                commandInsertBoleto.Parameters.AddWithValue("@IdPasajero", idPasajero);
                commandInsertBoleto.Parameters.AddWithValue("@IdRuta", idBoleto);
                commandInsertBoleto.Parameters.AddWithValue("@DestinoOrigen", origen);
                commandInsertBoleto.Parameters.AddWithValue("@DestinoFinal", destino);
                commandInsertBoleto.Parameters.AddWithValue("@FechaVenta", fechaVenta);
                commandInsertBoleto.Parameters.AddWithValue("@PrecioTotal", costo);

                commandInsertBoleto.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar la venta del ticket en la base de datos: " + ex.Message);
            }
            finally
            {
                miConexionSql.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.VentaTickets_SelectViaje selectViaje = new View.VentaTickets_SelectViaje();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Main.Content = selectViaje;

        }
    }
}
