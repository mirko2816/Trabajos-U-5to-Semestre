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
using System.Windows.Shapes;

namespace AppCalculadoraIntereses
{
    /// <summary>
    /// Lógica de interacción para InteresSimpleWindow.xaml
    /// </summary>
    public partial class InteresSimpleWindow : Window
    {
        public InteresSimpleWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CalcularInteres_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double monto = double.Parse(MontoTextBox.Text);
                double tasa = double.Parse(TasaTextBox.Text) / 100;
                int tiempo = int.Parse(TiempoTextBox.Text);

                double interes = monto * tasa * tiempo;

                ResultadoLabel.Content = $"Interés Simple: S/ {interes}";
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingresa valores válidos.");
            }
        }
    }
}
