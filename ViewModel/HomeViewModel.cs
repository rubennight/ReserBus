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
            string miConexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MuestraProximasSalidas();
            MuestraProximasLlegadas();
        }

        public void MuestraProximasSalidas()
        {
            string consulta = "select \r\n\tid_ruta,\r\n\tdestino,\r\n\tsalida,\r\n\tllegada,\r\n\tmax(numero_asientos)-max(total_pasajeros) as asientos_disponibles,\r\n\tstring_agg(ciudad,', ') as pasa_por\r\nfrom\r\n(\r\nSELECT\r\n\trvp.id_ruta,\r\n  first_value(d.ciudad) over (PARTITION BY rvp.id_ruta ORDER BY d.id_destino) AS destino,\r\n  vp.fecha_hora_salida AS salida,\r\n  vp.fecha_hora_llegada_estimada AS llegada,\r\n  u.numero_asientos,\r\n  total_pasajeros,\r\n  d.ciudad\r\n  FROM\r\n  [dbo v_1.3].rutas AS rvp\r\n  INNER JOIN [dbo v_1.3].destinos AS d ON d.id_destino = rvp.id_destino\r\n  INNER JOIN [dbo v_1.3].viajes_programados AS vp ON vp.id_viaje_programado = rvp.id_ruta\r\n  INNER JOIN [dbo v_1.3].unidades AS u ON u.id_unidad = vp.id_unidad\r\n  INNER JOIN (\r\n      SELECT\r\n      id_ruta,\r\n      COUNT (id_pasajero) AS total_pasajeros\r\n    FROM\r\n      [dbo v_1.3].boletos\r\n    GROUP BY\r\n      id_ruta\r\n  ) AS total_pasajeros_table ON total_pasajeros_table.id_ruta= vp.id_viaje_programado\r\n) sq\r\ngroup by\r\nid_ruta,\r\ndestino,\r\n\tsalida,\r\n\tllegada;";

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
            string consulta = "select\r\n\torigen,\r\n\thora_salida,\r\n\thora_llegada,\r\n\tSTRING_AGG(ciudad, ', ') as pasa_por\r\nfrom\r\n(\r\nSELECT\r\n\tFIRST_VALUE(d.ciudad) OVER (PARTITION BY id_ruta order by d.id_destino) + ', ' + FIRST_VALUE(d.estado) OVER (PARTITION BY id_ruta order by d.id_destino) as origen,\r\n\tvp.fecha_hora_salida as hora_salida,\r\n\tvp.fecha_hora_llegada_estimada as hora_llegada,\r\n\td.ciudad\r\nFROM\r\n\t[dbo v_1.3].rutas AS rvp\r\n\tinner join [dbo v_1.3].destinos d on d.id_destino=rvp.id_destino\r\n\tinner join [dbo v_1.3].viajes_programados vp on vp.id_viaje_programado=rvp.id_ruta\r\n) sq\r\ngroup by\r\n\torigen,\r\n\thora_salida,\r\n\thora_llegada;";

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
