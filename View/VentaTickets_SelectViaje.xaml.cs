using ReserBus.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
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
    /// Lógica de interacción para VentaTickets_SelectViaje.xaml
    /// </summary>
    public partial class VentaTickets_SelectViaje : Page
    {
        SqlConnection conexionSql;
        public VentaTickets_SelectViaje()
        {

            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);
            muestraOrigen();
            muestraDestino();
            muestraSucursal();
        }

        private void muestraOrigen()
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

        private void muestraDestino()
        {
            string consulta = "SELECT * FROM [dbo v_1.3].destinos";
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
            DataTable tablaResultado = new DataTable();
            adaptadorSql.Fill(tablaResultado);
            CBDestino.DisplayMemberPath = "ciudad";
            CBDestino.SelectedValuePath = "id_destino";
            CBDestino.ItemsSource = tablaResultado.DefaultView;
        }

        private void muestraSucursal()
        {
            string consulta = "SELECT * FROM [dbo v_1.3].destinos";
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(consulta, conexionSql);
            DataTable tablaResultado = new DataTable();
            adaptadorSql.Fill(tablaResultado);
            CBSucursal.ItemsSource = tablaResultado.DefaultView;
            CBSucursal.SelectedValuePath = "id_destino";
            CBSucursal.DisplayMemberPath = "ciudad";
        }

        private void muestraViajesCoincidentes(string origen, string destino,string fecha)
        {
            string consulta =
                "SELECT \r\n" +
                "subconsulta1.id_viaje_programado,\r\n" +
                "subconsulta1.fecha_hora_salida,\r\n" +
                "subconsulta1.fecha_hora_llegada_estimada,\r\n" +
                "subconsulta1.cupo,\r\n" +
                "subconsulta1.id_destino,\r\n" +
                "subconsulta2.id_destino\r\n\r\n" +
                "FROM\r\n" +
                "(select \r\n\t" +
                "v.id_viaje_programado,v.fecha_hora_salida,v.fecha_hora_llegada_estimada,v.cupo, r.id_destino\r\n\t" +
                "from\r\n\t" +
                "[dbo v_1.3].viajes_programados as v,\r\n\t" +
                "[dbo v_1.3].rutas as r \r\n\t" +
                "where\r\n\t" +
                "v.id_viaje_programado = r.id_ruta and\r\n\t" +
                "r.id_destino = @origen) AS subconsulta1\r\n" +
                "INNER JOIN\r\n" +
                "(select \r\n\t" +
                "v.id_viaje_programado,r.id_destino\r\n\t" +
                "from\r\n\t" +
                "[dbo v_1.3].viajes_programados as v,\r\n\t" +
                "[dbo v_1.3].rutas as r \r\n\t" +
                "where\r\n\t" +
                "v.id_viaje_programado = r.id_ruta and\r\n\t" +
                "r.id_destino = @destino) AS subconsulta2\r\n\r\n" +
                "ON\r\n" +
                "subconsulta1.id_viaje_programado = subconsulta2.id_viaje_programado " +
                "WHERE\r\n" +
                "CONVERT(DATE, subconsulta1.fecha_hora_salida) = @fecha";
            SqlCommand comando = new SqlCommand(consulta, conexionSql);
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(comando);
            DataTable tablaResultado = new DataTable();//Viajes coincidentes.
            comando.Parameters.AddWithValue("origen", origen);
            comando.Parameters.AddWithValue("destino", destino);
            comando.Parameters.AddWithValue("fecha", fecha);
            
            adaptadorSql.Fill(tablaResultado);
            DGViajesCoincidentes.ItemsSource = tablaResultado.DefaultView;
            DGViajesCoincidentes.SelectedValuePath = "id_viaje_programado";
            tablaResultado.Columns.Add("ruta", typeof(string));
            tablaResultado.Columns.Add("precio", typeof(string));
            
            List<string> viajesCoincidentes = new List<string>();//Id's de los viajes coincidentes
            foreach (DataRow row in tablaResultado.Rows)
            {
                viajesCoincidentes.Add(row["id_viaje_programado"].ToString());
            }
            DataTable rutaYPrecios = obtenRutaYPrecio(viajesCoincidentes);//Rutas y precios de los viajes coincidentes.
            int i = 0;
            foreach (var item in tablaResultado.Rows )
            {
                tablaResultado.Rows[i]["ruta"] = rutaYPrecios.Rows[i][0];//Agregamos la ruta completa al viaje coincidente.
                tablaResultado.Rows[i]["precio"] = rutaYPrecios.Rows[i][1];//Agregamos el precio completo al viaje coincidente.

                i++;
            }
            

        }

        private DataTable obtenRutaYPrecio(List<string> viajes)
        {
            string consulta = "\r\nSELECT STRING_AGG(d.ciudad,',') AS ruta_total, STRING_AGG(d.precio_base,',') AS precio_total\r\n" +
                "FROM [dbo v_1.3].rutas AS r\r\n" +
                "INNER JOIN [dbo v_1.3].destinos AS d ON d.id_destino = r.id_destino AND r.id_ruta = @viaje;";
            SqlCommand comandoSQL = new SqlCommand(consulta, conexionSql);
            SqlDataAdapter adaptadorSql = new SqlDataAdapter(comandoSQL);
            DataTable tablaResultado = new DataTable();//Tabla de las rutas y precios de todos los viajes coincidentes.
            foreach (string viaje in viajes)
            {
                using (adaptadorSql)
                {
                    comandoSQL.Parameters.AddWithValue("@viaje", viaje);
                    adaptadorSql.Fill(tablaResultado);
                    //Una vez ejecutada la consulta se recorrera el DT para ver que escalas de la ruta principal pertenecen al viaje

                }

            }
            return tablaResultado;
        }
    

        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(CBOrigen.SelectedIndex.ToString());
            Console.WriteLine(CBDestino.SelectedIndex.ToString());
            Console.WriteLine(DPFecha.SelectedDate.Value.ToString("yyyy/MM/dd"));

            muestraViajesCoincidentes(CBOrigen.SelectedValue.ToString(), CBDestino.SelectedValue.ToString(), DPFecha.SelectedDate.Value.ToString("yyyy/MM/dd"));
        }

        private void handleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                // Reemplazar el modelo
                var selectedItem = dataGrid.SelectedItem;

                // Convierte el elemento seleccionado al tipo de objeto correspondiente (si es necesario)
                // Por ejemplo, si el DataGrid está enlazado a una colección de objetos de tipo 'MyItem', puedes hacer lo siguiente:
                // var selectedMyItem = (MyItem)selectedItem;

                // Ahora puedes acceder a las propiedades del elemento seleccionado y utilizarlas como desees
                // Por ejemplo, puedes mostrarlas en un MessageBox
                DataRowView rowView = (DataRowView)dataGrid.SelectedItem;
                MessageBox.Show($"Elemento seleccionado: {rowView["id_viaje_programado"].ToString()}");

            }

        }
    }
}
