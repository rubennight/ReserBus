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
    /// Lógica de interacción para Horarios.xaml
    /// </summary>
    public partial class Horarios : Page
    {
        SqlConnection conexionSql;
        public Horarios()
        {
            InitializeComponent();
            string conexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(conexion);
            llenaViajesPendientes();
            
        }

        private void llenaViajesPendientes()
        {
            string consulta = "SELECT DISTINCT v.id_viaje_programado,\r\n" +
                "       FIRST_VALUE(d.ciudad) OVER(PARTITION BY v.id_viaje_programado ORDER BY v.fecha_hora_salida ASC) AS primera_ciudad,\r\n" +
                "       LAST_VALUE(d.ciudad) OVER(PARTITION BY v.id_viaje_programado ORDER BY v.fecha_hora_salida ASC RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) AS ultima_ciudad,\r\n" +
                "       FIRST_VALUE(v.fecha_hora_salida) OVER(PARTITION BY v.id_viaje_programado ORDER BY v.fecha_hora_salida ASC) AS primera_fecha_hora_salida,\r\n" +
                "       \r\n       " +
                "FIRST_VALUE(u.modelo) OVER(PARTITION BY v.id_viaje_programado ORDER BY v.fecha_hora_salida ASC) AS primer_modelo\r\n" +
                "FROM [dbo v_1.3].viajes_programados AS v\r\n" +
                "INNER JOIN [dbo v_1.3].rutas r ON v.id_viaje_programado = r.id_ruta\r\n" +
                "INNER JOIN [dbo v_1.3].destinos d ON d.id_destino = r.id_destino\r\n" +
                "INNER JOIN [dbo v_1.3].unidades u ON u.id_unidad = v.id_unidad\r\n" +
                "WHERE CONVERT(DATE, v.fecha_hora_salida) >= CONVERT(DATE,GETDATE())";
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionSql);
                DataTable tablaResultado = new DataTable();
                adaptador.Fill(tablaResultado);
                DGViajesProgramados.SelectedValuePath = "id_destino";
                DGViajesProgramados.ItemsSource = tablaResultado.DefaultView;
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
    }
}
