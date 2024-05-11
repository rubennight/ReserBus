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
        public Config()
        {
            InitializeComponent();
        }

        private void Open_Unidades_Form(object sender, RoutedEventArgs e)
        {
            UnidadesForm.IsOpen= UnidadesForm.IsOpen ? false : true;
            Overlay1.Visibility= UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility= UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
        }

        private void Toggle_Unidades_Form(object sender, RoutedEventArgs e)
        {
            UnidadesForm.IsOpen = UnidadesForm.IsOpen ? false : true;
            Overlay1.Visibility = UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility = UnidadesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
        }

        private void Toggle_Conductores_Form(object sender, RoutedEventArgs e)
        {
            ChoferesForm.IsOpen = ChoferesForm.IsOpen ? false : true;
            Overlay1.Visibility = ChoferesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
            Overlay2.Visibility = ChoferesForm.IsOpen ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
