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
using LiveCharts;
using LiveCharts.Wpf;

namespace AppCalculadoraIntereses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Simulacion
    {
        public int Periodo { get; set; }
        public double CapitalInicial { get; set; }
        public double Interes { get; set; }
        public double CapitalAcumulado { get; set; }
    }

    public partial class MainWindow : Window
    {
        // Series de datos para los gráficos
        public ChartValues<double> InteresSimpleSeries { get; set; }
        public ChartValues<double> InteresCompuestoSeries { get; set; }
        public List<string> LabelsAnios { get; set; }

        // Listas para las tablas de simulación
        public List<Simulacion> SimulacionInteresSimple { get; set; }
        public List<Simulacion> SimulacionInteresCompuesto { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InteresSimpleSeries = new ChartValues<double>();
            InteresCompuestoSeries = new ChartValues<double>();
            LabelsAnios = new List<string>();

            SimulacionInteresSimple = new List<Simulacion>();
            SimulacionInteresCompuesto = new List<Simulacion>();

            DataContext = this;
        }

        // Evento para calcular el interés simple
        private void CalcularInteresSimple(object sender, RoutedEventArgs e)
        {
            try
            {
                double capital = Convert.ToDouble(txtCapitalSimple.Text);
                double tasa = Convert.ToDouble(txtTasaSimple.Text) / 100;
                double anios = Convert.ToDouble(txtAniosSimple.Text);

                InteresSimpleSeries.Clear();
                LabelsAnios.Clear();

                for (int i = 0; i <= anios; i++)
                {
                    double capitalInicial = capital;
                    double interes = capital * tasa * i;
                    double capitalAcumulado = capital + interes;

                    InteresSimpleSeries.Add(capitalAcumulado);
                    LabelsAnios.Add(i.ToString());

                    // Agregamos los datos a la tabla de simulación
                    SimulacionInteresSimple.Add(new Simulacion
                    {
                        Periodo = i,
                        CapitalInicial = capitalInicial,
                        Interes = interes,
                        CapitalAcumulado = capitalAcumulado
                    });
                }

                // Margenes arreglados
                chartInteresSimple.AxisY[0].MinValue = InteresSimpleSeries.Min();
                chartInteresSimple.AxisY[0].MaxValue = InteresSimpleSeries.Max();


                dataGridSimple.ItemsSource = SimulacionInteresSimple;
                double capitalFinal = Math.Round(InteresSimpleSeries[(int)anios],4);
                txtResultadoSimple.Opacity = 1;
                txtResultadoSimple.Text = $"Capital Final: S/. {InteresSimpleSeries[(int)anios]}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en los datos ingresados. Verifique los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento para calcular el interés compuesto
        private void CalcularInteresCompuesto(object sender, RoutedEventArgs e)
        {
            try
            {
                double capital = Convert.ToDouble(txtCapitalCompuesto.Text);
                double tasa = Convert.ToDouble(txtTasaCompuesto.Text) / 100;
                double anios = Convert.ToDouble(txtAniosCompuesto.Text);

                InteresCompuestoSeries.Clear();
                LabelsAnios.Clear();

                for (int i = 0; i <= anios; i++)
                {
                    double capitalInicial = capital;
                    double valorFuturo = capital * Math.Pow((1 + tasa), i);
                    double interes = valorFuturo - capital;

                    InteresCompuestoSeries.Add(valorFuturo);
                    LabelsAnios.Add(i.ToString());

                    // Agregamos los datos a la tabla de simulación
                    SimulacionInteresCompuesto.Add(new Simulacion
                    {
                        Periodo = i,
                        CapitalInicial = capitalInicial,
                        Interes = interes,
                        CapitalAcumulado = valorFuturo
                    });
                }

                double valorFuturoRedon = Math.Round(InteresCompuestoSeries[(int)anios], 4);
                double interesRedon = Math.Round(InteresCompuestoSeries[(int)anios] - capital, 4);

                // Margenes arreglados
                chartInteresCompuesto.AxisY[0].MinValue = InteresCompuestoSeries.Min();
                chartInteresCompuesto.AxisY[0].MaxValue = InteresCompuestoSeries.Max();

                dataGridCompuesto.ItemsSource = SimulacionInteresCompuesto;
                txtResultadoCompuesto.Opacity = 1;
                txtResultadoCompuesto.Text = $"Interés: S/. {interesRedon}, Valor futuro: S/. {valorFuturoRedon}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en los datos ingresados. Verifique los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BorrarDatosInteresSimple(object sender, RoutedEventArgs e)
        {
            txtCapitalSimple.Text = "";
            txtTasaSimple.Text = "";
            txtAniosSimple.Text = "";
        }

        private void BorrarDatosInteresCompuesto(object sender, RoutedEventArgs e)
        {
            txtCapitalCompuesto.Text = "";
            txtTasaCompuesto.Text = "";
            txtAniosCompuesto.Text = "";
        }
    }
}