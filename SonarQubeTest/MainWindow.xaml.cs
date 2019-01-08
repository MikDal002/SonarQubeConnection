using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
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
using Newtonsoft.Json.Converters;

namespace SonarQubeTest
{
    public class MainWindowVM
    {
        public ComplexFactorVM NeedToSplitClasses { get; } = new ComplexFactorVM()
        {
            Name = "Potrzeba rozbicia klas",
            Description =
                "Paramtetr określający na ile klasy obecne w projekcie są przerośnięte lub nadmiernie skomplikowane i mogą w przyszłości utrudniać zrozumienie kodu oraz rozwój aplikacji.",
        };

        public ComplexFactorVM ProblematicOfFutureDevelopment { get; } = new ComplexFactorVM()
        {
            Name = "Problematyka dalszego rozwoju",
            Description =
                "Parametr ten okresla na ile problematyczny może być dalszy rozwoj aplikacji. Uwzglednione są wszelkie złe praktyki stosowane w kodzie oraz ilość zagnieżdżeń różnego rodzaju pętli i instrukcji warunkowych",
        };

        public ComplexFactorVM SimplicityOfCode { get; } = new ComplexFactorVM()
        {
            Name = "Prostota kodu",
            Description =
                "Prostota bieżącego stanu projektu mówi o tym, czy klasy i metody mają odpowiednią wielkość.",
        };

        public void AddDataFromSonarQube(Dictionary<DateTimeOffset, Dictionary<string, double>> data)
        {
            foreach (var measure in data)
            {
                AddSingleDataFromSonarQube(measure.Key, measure.Value);
            }
        }

        public void AddSingleDataFromSonarQube(DateTimeOffset time, Dictionary<string, double> measures)
        {
            var splitClasses = new ComplexFactor()
            {
                Time = time,
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Złożoność cyklomatyczna",
                        CurrentValue = measures[SonarQubeConnection.Complexity] / 10,
                        IdealValue = 10
                    },
                    new SimpleFactor()
                    {
                        Name = "Duplikacja kodu",
                        CurrentValue = measures[SonarQubeConnection.DuplicatedLinesDensity],
                        Kind = ValueType.Percentage,
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość linii na klasę",
                        CurrentValue = measures[SonarQubeConnection.Lines] / measures[SonarQubeConnection.Classes],
                        IdealValue = 330
                    }
                },
            };
            var futureDevelopment = new ComplexFactor()
            {
                Time = time,
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Złożoność cyklomatyczna",
                        CurrentValue = measures[SonarQubeConnection.Complexity] / 10,
                        IdealValue = 10
                    },
                    new SimpleFactor()
                    {
                        Name = "Zapachy kodu",
                        CurrentValue = measures[SonarQubeConnection.CodeSmells]  / 10,
                        IdealValue = 30
                    },
                    new SimpleFactor()
                    {
                        Name = "Podatności",
                        CurrentValue = measures[SonarQubeConnection.Vulnerabilities],
                        IdealValue = 10
                    }
                },
            };

            var simplicityOfCode = new ComplexFactor()
            {
                Time = time,
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Ilość linii kodu na klasę",
                        CurrentValue = measures[SonarQubeConnection.Lines] / measures[SonarQubeConnection.Classes],
                        IdealValue = 330
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość linii kodu na metodę",
                        CurrentValue = measures[SonarQubeConnection.Lines] / measures[SonarQubeConnection.Functions],
                        IdealValue = 25
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość metod publicznych na klasę",
                        CurrentValue = measures[SonarQubeConnection.Functions] / measures[SonarQubeConnection.Classes],
                        IdealValue = 12
                    }
                }
            };
            NeedToSplitClasses.ComplexFactors.Add(splitClasses);
            ProblematicOfFutureDevelopment.ComplexFactors.Add(futureDevelopment);
            SimplicityOfCode.ComplexFactors.Add(simplicityOfCode);
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public new MainWindowVM DataContext { get => base.DataContext as MainWindowVM; set => base.DataContext = value; }
        public MainWindow()
        {
            var boo = new MainWindowVM();
            var sqConnection = new SonarQubeConnection();
            try
            {
                var data = sqConnection.Ask();
                var obs = data.ToObservable().Delay(TimeSpan.FromSeconds(2));
                obs.Subscribe(d =>
                {
                    Thread.Sleep(300);
                    boo.AddSingleDataFromSonarQube(d.Key, d.Value);
                });
            }
            catch (SonarQubeConnectionException exce)
            {
                MessageBox.Show("Nie udało się pobrać danych z SonarQube.", "Błąd połaczenia z SonarQube",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }


            InitializeComponent();
            DataContext = boo;
        }
    }
}
