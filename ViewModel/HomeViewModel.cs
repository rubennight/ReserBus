using ReserBus.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserBus.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private List<ProximaSalida> _salidasData;
        public List<ProximaSalida> SalidasData
        {
            get {  return _salidasData; }
            set 
            { 
                _salidasData = value;
                OnPropertyChanged(nameof(SalidasData));
            }
        }

        private List<ProximaLlegada> _llegadasData;

        public List<ProximaLlegada> LlegadasData
        {
            get { return _llegadasData; }
            set
            {
                _llegadasData = value;
                OnPropertyChanged(nameof(_llegadasData));
            }
        }

        SqlConnection miConexionSql;

        public event PropertyChangedEventHandler PropertyChanged;

        public HomeViewModel()
        {
            string miConexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.ReserBusConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MuestraProximasSalidas();
            MuestraProximasLlegadas();
        }

        public void MuestraProximasSalidas()
        {
            string consulta = "SELECT\r\n\trvp.nombre_ruta AS destino,\r\n\tvp.fecha_hora_salida AS salida,\r\n\tvp.fecha_hora_llegada_estimada AS llegada,\r\n\tMAX ( u.numero_asientos ) - MAX ( total_pasajeros ) AS asientos_disponibles,\r\n\tSTRING_AGG ( d.ciudad, ', ' ) AS pasa_por \r\nFROM\r\n\truta_viaje_programado AS rvp\r\n\tINNER JOIN destinos AS d ON d.id_destino = rvp.id_destino\r\n\tINNER JOIN viajes_programados AS vp ON vp.id_viaje_programado = rvp.id_ruta_viaje_programado\r\n\tINNER JOIN unidades AS u ON u.id_unidad = vp.id_unidad\r\n\tINNER JOIN ( SELECT id_viaje_programado, COUNT ( id_pasajero ) AS total_pasajeros FROM boletos GROUP BY id_viaje_programado ) AS total_pasajeros_table ON total_pasajeros_table.id_viaje_programado = vp.id_viaje_programado \r\nGROUP BY\r\n\trvp.nombre_ruta,\r\n\tvp.fecha_hora_salida,\r\n\tvp.fecha_hora_llegada_estimada;";

            SqlDataAdapter adapter = new SqlDataAdapter(consulta, miConexionSql);

            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable); 

            List<ProximaSalida> salidas = new List<ProximaSalida>();

            foreach (DataRow row in dataTable.Rows)
            {
                ProximaSalida salida = new ProximaSalida
                {
                    Destino = row["destino"].ToString(),
                    Salida = Convert.ToDateTime(row["salida"]),
                    Llegada = Convert.ToDateTime(row["llegada"]),
                    LugaresLibres = Convert.ToInt32(row["asientos_disponibles"]),
                    Escalas = row["pasa_por"].ToString()
                };

                salidas.Add(salida);
            }

            SalidasData = salidas;
        }

        public void MuestraProximasLlegadas()
        {
            string consulta = "SELECT \r\n\td.ciudad + ', ' + d.estado AS origen,\r\n\tvp.fecha_hora_salida AS hora_salida,\r\n\tvp.fecha_hora_llegada_estimada AS hora_llegada,\r\n\td_2.ciudad + ', ' + d_3.ciudad + ' y ' + d_4.ciudad AS pasa_por \r\nFROM \r\n\truta_viaje_programado AS rvp\r\n\tINNER JOIN destinos AS d ON d.id_destino = rvp.id_destino \r\n\tAND rvp.destino_posicion = 1\r\n\tINNER JOIN viajes_programados AS vp ON vp.id_viaje_programado = rvp.id_ruta_viaje_programado \r\n\tAND rvp.destino_posicion = 1\r\n\tLEFT JOIN ruta_viaje_programado AS rvp_2 ON rvp_2.id_ruta_viaje_programado = rvp.id_ruta_viaje_programado \r\n\tAND rvp_2.destino_posicion = 2\r\n\tLEFT JOIN destinos AS d_2 ON d_2.id_destino = rvp_2.id_destino\r\n\tLEFT JOIN ruta_viaje_programado AS rvp_3 ON rvp_3.id_ruta_viaje_programado = rvp.id_ruta_viaje_programado \r\n\tAND rvp_3.destino_posicion = 3\r\n\tLEFT JOIN destinos AS d_3 ON d_3.id_destino = rvp_3.id_destino\r\n\tLEFT JOIN ruta_viaje_programado AS rvp_4 ON rvp_4.id_ruta_viaje_programado = rvp.id_ruta_viaje_programado \r\n\tAND rvp_4.destino_posicion = 4\r\n\tLEFT JOIN destinos AS d_4 ON d_4.id_destino = rvp_4.id_destino;";

            SqlDataAdapter adapter = new SqlDataAdapter(consulta, miConexionSql);

            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            List<ProximaLlegada> llegadas = new List<ProximaLlegada>();

            foreach (DataRow row in dataTable.Rows)
            {
                ProximaLlegada llegada = new ProximaLlegada()
                {
                    Origen = row["origen"].ToString(),
                    Salida = Convert.ToDateTime(row["hora_salida"]),
                    Llegada = Convert.ToDateTime(row["hora_llegada"]),
                    Escalas = row["pasa_por"].ToString()
                };
                llegadas.Add(llegada);
            }
            LlegadasData = llegadas;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
