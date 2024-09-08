using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppCalculadoraIntereses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InteresSimple_Click(object sender, RoutedEventArgs e)
        {
            // Abrir la ventana de cálculo de interés simple
            InteresSimpleWindow simpleWindow = new InteresSimpleWindow();
            simpleWindow.Show();
            this.Close(); // Cierra el menú principal
        }

        private void InteresCompuesto_Click(object sender, RoutedEventArgs e)
        {
            // IMPLEMENTAREMOS LA FUNCIONALIDAD DEL INTERES COMPUESTO PRONTO
        }
    }
}