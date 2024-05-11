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

        private List<Conductor> _conductores;

        public List<Conductor> ConductoresData
        {
            get { return _conductores; }
            set { 
                    _conductores = value;
                    OnPropertyChanged(nameof(_conductores));
                }
        }
        private List<Unidad> _unidad;
        public List<Unidad> UnidadesData
        {
            get { return _unidad;}
            set { 
                    _unidad = value; 
                    OnPropertyChanged(nameof(_unidad));
                }
        }

        SqlConnection miConexionSql;

        public event PropertyChangedEventHandler PropertyChanged;

        public Config()
        {
            InitializeComponent();

            DataContext = this;

            string miConexion = ConfigurationManager.ConnectionStrings["ReserBus.Properties.Settings.dbo_v_1_3ConnectionString"].ConnectionString;
        
            miConexionSql = new SqlConnection(miConexion);

            MuestraConductores();
            MuestraUnidades();
        }

        public void MuestraUnidades()
        {
            string consulta = "SELECT\n\tu.modelo,\n\tu.numero_asientos\nFROM\n\t[dbo v_1.3].unidades AS u";
        
            SqlDataAdapter adapter  = new SqlDataAdapter(consulta, miConexionSql);

            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            List<Unidad> unidades = new List<Unidad>();

            foreach (DataRow row in dataTable.Rows)
            {
                Unidad unidad = new Unidad
                {
                    Modelo = row["modelo"].ToString(),
                    NumeroAsientos = Convert.ToInt32(row["numero_asientos"])
                };

                unidades.Add(unidad);
            }

            UnidadesData = unidades;
        }

        public void MuestraConductores()
        {
            string consulta = "SELECT\r\n\tc.nombre,\r\n\tc.apellidos \r\nFROM\r\n\t[dbo v_1.3].choferes AS c";

            SqlDataAdapter adapter = new SqlDataAdapter(consulta, miConexionSql);

            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            List<Conductor> conductores = new List<Conductor>();

            foreach (DataRow row in dataTable.Rows)
            {
                Conductor conductor = new Conductor
                {
                    Nombre = row["nombre"].ToString(),
                    Apellidos = row["apellidos"].ToString()
                };

                conductores.Add(conductor);
            }

            ConductoresData = conductores;
        }

        private void Toggle_Unidades_Form()
        {
            UnidadesForm.IsOpen = UnidadesForm.IsOpen ? false : true;
            Overlay1.Visibility = UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility = UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            if (!UnidadesForm.IsOpen)
            {
                Reset_Forms();
                createRegistro = false;
            }
        }

        private void Toggle_Conductores_Form()
        {
            ChoferesForm.IsOpen = ChoferesForm.IsOpen ? false : true;
            Overlay1.Visibility = ChoferesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility = ChoferesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            if (!ChoferesForm.IsOpen)
            {
                Reset_Forms();
                createRegistro = false;
            }
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
                   if (dataGrid.SelectedItem is Unidad unidadSelected)
                   {
                        Toggle_Unidades_Form();
                        txtModelo.Text = unidadSelected.Modelo;
                        txtCantidadAsientos.Text = unidadSelected.NumeroAsientos.ToString();
                
                   }
            }


        }

        private void SetConductoresForm(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                if (dataGrid.SelectedItem is Conductor conductorSelected)
                {
                    Toggle_Conductores_Form();
                    txtNombre.Text = conductorSelected.Nombre;
                    txtApellidos.Text = conductorSelected.Apellidos;
                
                }
            }


        }

        private void CrearConductor(object sender, RoutedEventArgs e)
        {
            Toggle_Conductores_Form();
            Reset_Forms();
            createRegistro = true;
        }

        private void CrearUnidad(object sender, RoutedEventArgs e)
        {
            Toggle_Unidades_Form();
            Reset_Forms();
            createRegistro = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Toggle_Conductores_Form();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Toggle_Unidades_Form();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
