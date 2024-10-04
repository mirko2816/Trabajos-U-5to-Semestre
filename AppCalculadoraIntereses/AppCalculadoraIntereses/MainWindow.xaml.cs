using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Converters;
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

    public class SimulacionAmortiguamiento
    {
        public int periodo { get; set; }
        public double interes { get; set; }
        public double amortizacion { get; set; }
        public double deudaRestante { get; set; }
    }
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
        public ChartValues<double> amortiguamientoFrancesSeries { get; set; }
        public ChartValues<double> interesFrancesSeries { get; set; }
        public List<string> LabelsAnios { get; set; }

        // Listas para las tablas de simulación
        public List<Simulacion> SimulacionInteresSimple { get; set; }
        public List<Simulacion> SimulacionInteresCompuesto { get; set; }
        public List<SimulacionAmortiguamiento> SimulacionAmortiguamiento { get; set; }
        public Func<double, string> FormatoLabelsEjeY { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InteresSimpleSeries = new ChartValues<double>();
            InteresCompuestoSeries = new ChartValues<double>();
            amortiguamientoFrancesSeries = new ChartValues<double>();
            interesFrancesSeries = new ChartValues<double>();
            LabelsAnios = new List<string>();

            FormatoLabelsEjeY = value => $"S/. {value:N2}";

            DataContext = this;
        }

        
        // Actualiza constantemente el resultado
        private void ActualizarSimulacionInteresSimple(object sender, TextChangedEventArgs e)
        {
            CalcularInteresSimple();
        }

        // Evento para calcular el interés simple
        private void CalcularInteresSimple()
        {
            try
            {
                // No hace nada si alguno de los tres campos esta vacio
                if (string.IsNullOrEmpty(txtCapitalSimple.Text) ||
                    string.IsNullOrEmpty(txtTasaSimple.Text) ||
                    string.IsNullOrEmpty(txtAniosSimple.Text) )
                {
                    return;
                }

                double capital = Convert.ToDouble(txtCapitalSimple.Text);
                double tasa = Convert.ToDouble(txtTasaSimple.Text) / 100;
                double anios = Convert.ToDouble(txtAniosSimple.Text);

                InteresSimpleSeries.Clear();
                LabelsAnios.Clear();
                SimulacionInteresSimple = new List<Simulacion>();

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
                double capitalFinal = Math.Round(InteresSimpleSeries[(int)anios],2);
                txtResultadoSimple.Opacity = 1;
                txtResultadoSimple.Text = $"Capital Final: S/. {InteresSimpleSeries[(int)anios]}";
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

            if (InteresSimpleSeries != null)
            {
                InteresSimpleSeries.Clear();
            }
            
            if (SimulacionInteresSimple != null)
            {
                SimulacionInteresSimple.Clear();
                dataGridSimple.ItemsSource = null;
            }
            
        }

        // Actualiza constantemente el resultado
        private void ActualizarSimulacionInteresCompuesto(object sender, TextChangedEventArgs e)
        {
            CalcularInteresCompuesto();
        }

        // Evento para calcular el interés compuesto
        private void CalcularInteresCompuesto()
        {
            try
            {

                // No hace nada si alguno de los tres campos esta vacio
                if (string.IsNullOrEmpty(txtCapitalCompuesto.Text) ||
                    string.IsNullOrEmpty(txtTasaCompuesto.Text) ||
                    string.IsNullOrEmpty(txtAniosCompuesto.Text))
                {
                    return;
                }
                double capital = Convert.ToDouble(txtCapitalCompuesto.Text);
                double tasa = Convert.ToDouble(txtTasaCompuesto.Text) / 100;
                double anios = Convert.ToDouble(txtAniosCompuesto.Text);

                InteresCompuestoSeries.Clear();
                LabelsAnios.Clear();
                SimulacionInteresCompuesto = new List<Simulacion>();

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
                        CapitalInicial = Math.Round(capitalInicial,2),
                        Interes = Math.Round(interes,2),
                        CapitalAcumulado = Math.Round(valorFuturo, 2)
                    });
                }

                double valorFuturoRedon = Math.Round(InteresCompuestoSeries[(int)anios], 2);
                double interesRedon = Math.Round(InteresCompuestoSeries[(int)anios] - capital, 2);

                // Margenes arreglados
                chartInteresCompuesto.AxisY[0].MinValue = InteresCompuestoSeries.Min();
                chartInteresCompuesto.AxisY[0].MaxValue = InteresCompuestoSeries.Max();

                dataGridCompuesto.ItemsSource = SimulacionInteresCompuesto;
                txtResultadoCompuesto.Opacity = 1;
                txtResultadoCompuesto.Text = $"Interés: S/. {interesRedon}\t\tValor futuro: S/. {valorFuturoRedon}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en los datos ingresados. Verifique los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BorrarDatosInteresCompuesto(object sender, RoutedEventArgs e)
        {
            txtCapitalCompuesto.Text = "";
            txtTasaCompuesto.Text = "";
            txtAniosCompuesto.Text = "";
            if (InteresCompuestoSeries != null)
            {
                InteresCompuestoSeries.Clear();
            }

            if (SimulacionInteresCompuesto != null)
            {
                SimulacionInteresCompuesto.Clear();
                dataGridCompuesto.ItemsSource = null;
            }
            
        }

        // Actualiza constantemente el resultado
        private void ActualizarSimulacionAmortiguamiento(object sender, TextChangedEventArgs e)
        {
            CalcularAmortiguamiento();
        }

        // Evento para calcular el amortiguamiento
        private void CalcularAmortiguamiento()
        {
            try
            {
                // No hace nada si alguno de los tres campos esta vacio
                if (string.IsNullOrEmpty(txtCapitalAmortiguamiento.Text) ||
                    string.IsNullOrEmpty(txtTasaAmortiguamiento.Text) ||
                    string.IsNullOrEmpty(txtMesesAmortiguamiento.Text))
                {
                    return;
                }

                double capital = Convert.ToDouble(txtCapitalAmortiguamiento.Text);
                double tasa = Convert.ToDouble(txtTasaAmortiguamiento.Text) / 100;
                double meses = Convert.ToDouble(txtMesesAmortiguamiento.Text);

                amortiguamientoFrancesSeries.Clear();
                interesFrancesSeries.Clear();
                LabelsAnios.Clear();
                SimulacionAmortiguamiento = new List<SimulacionAmortiguamiento>();

                double tasaMensual = Math.Round(tasa / 12, 4);
                double cuotaMensual = Math.Round((capital * tasaMensual * Math.Pow((1 + tasaMensual), meses)) / (Math.Pow((1 + tasaMensual), meses) - 1), 4);
                double deudaRestante = Math.Round(capital, 4);

                txtCuotaMensual.Text = "Cuota mensual fija: S/." + cuotaMensual.ToString();
                txtCuotaMensual.Opacity = 1;

                for (int i = 1; i <= meses; i++)
                {
                    double interes = deudaRestante * tasaMensual;
                    double amortiguamiento = cuotaMensual - interes;
                    deudaRestante -= amortiguamiento;

                    interesFrancesSeries.Add(interes);
                    amortiguamientoFrancesSeries.Add(amortiguamiento);
                    LabelsAnios.Add(i.ToString());

                    SimulacionAmortiguamiento.Add(new SimulacionAmortiguamiento
                    {
                        periodo = i,
                        amortizacion = Math.Round(amortiguamiento, 4),
                        interes = Math.Round(interes, 4),
                        deudaRestante = Math.Round(deudaRestante, 4),
                    }); 
                }

                dataGridAmortiguamiento.ItemsSource = SimulacionAmortiguamiento;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error en los datos ingresados. Verifique los Campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BorrarDatosAmortiguamiento(object sender, RoutedEventArgs e)
        {
            txtCapitalAmortiguamiento.Text = "";
            txtTasaAmortiguamiento.Text = "";
            txtMesesAmortiguamiento.Text = "";
            if (amortiguamientoFrancesSeries != null)
            {
                amortiguamientoFrancesSeries.Clear();
            }
            
            if (interesFrancesSeries != null)
            {
                interesFrancesSeries.Clear();
            }
            
            if (SimulacionAmortiguamiento != null)
            {
                SimulacionAmortiguamiento.Clear();
                dataGridAmortiguamiento.ItemsSource = null;
            }
            
        }

    }
}