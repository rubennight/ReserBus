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
        string idViajeProgramado;

        public VentaTickets_Formulario(DataRowView selectedItem)
        {
            InitializeComponent();

            DataContext = this;

            string miConexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            txtOrigen.Text = selectedItem["origen"].ToString();
            txtDestino.Text = selectedItem["destino"].ToString();
            txtFechaSalida.Text = selectedItem["fecha_hora_salida"].ToString();
            txtCosto.Text = "$ " + selectedItem["precio"].ToString();
            idViajeProgramado = selectedItem["id_viaje_programado"].ToString();
        }

        public void InsertarVentaTicket(string nombre, string apellidos, Int64 telefono, string origen, string destino, DateTime fechaSalida, Decimal costo, string idViajeProgramado)
        {
            //primero generamos un id para el boleto, id para el pasajero y luego la fecha actual:
            Guid idBoleto = Guid.NewGuid();
            Guid idPasajero = Guid.NewGuid();
            Guid idRuta;
            DateTime fechaVenta = DateTime.Now;

            if (!Guid.TryParse(idViajeProgramado, out idRuta))
            {
                MessageBox.Show("El idViajeProgramado no es un GUID válido.");
                return;
            }

            try
            {
                miConexionSql.Open();
                //primero insertamos al pasajero 
                string queryInsertPasajero = "INSERT INTO [dbo v_1.3].pasajeros (id_pasajero, nombre, apellidos, telefono) VALUES (@IdPasajero, @Nombre, @Apellidos, @Telefono)"; ;
                SqlCommand commandInsertPasajero = new SqlCommand(queryInsertPasajero, miConexionSql);

                commandInsertPasajero.Parameters.AddWithValue("@IdPasajero", idPasajero);
                commandInsertPasajero.Parameters.AddWithValue("@Nombre", nombre);
                commandInsertPasajero.Parameters.AddWithValue("@Apellidos", apellidos);
                commandInsertPasajero.Parameters.AddWithValue("@Telefono", telefono);

                commandInsertPasajero.ExecuteNonQuery();

                string queryInsertBoleto = "INSERT INTO [dbo v_1.3].boletos (id_boleto, id_pasajero, id_ruta, destino_origen_nombre, destino_final_nombre, fecha_venta, precio_total) \r\n VALUES (@IdBoleto, @IdPasajero, @IdRuta, @DestinoOrigen, @DestinoFinal, @FechaVenta, @PrecioTotal)";

                SqlCommand commandInsertBoleto = new SqlCommand(queryInsertBoleto, miConexionSql);

                commandInsertBoleto.Parameters.AddWithValue("@IdBoleto", idBoleto);
                commandInsertBoleto.Parameters.AddWithValue("@IdPasajero", idPasajero);
                commandInsertBoleto.Parameters.AddWithValue("@IdRuta", idRuta);
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

                View.Home home= new View.Home();
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                mainWindow.Main.Content = home;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            View.VentaTickets_SelectViaje selectViaje = new View.VentaTickets_SelectViaje();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Main.Content = selectViaje;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            string nombre = txtNombre.Text;
            string apellidos = txtApellidos.Text;
            Int64 telefono = txtTelefono.Text == "" ? 0 : Convert.ToInt64(txtTelefono.Text);
            string origen = txtOrigen.Text;
            string destino = txtDestino.Text;
            DateTime fechaSalida = Convert.ToDateTime(txtFechaSalida.Text);
            Decimal costo = Convert.ToDecimal(txtCosto.Text.Replace("$", "")); // Suponiendo que el texto incluye el símbolo '$'

            InsertarVentaTicket(nombre, apellidos, telefono, origen, destino, fechaSalida, costo, idViajeProgramado);
            }
            
        }
    }

