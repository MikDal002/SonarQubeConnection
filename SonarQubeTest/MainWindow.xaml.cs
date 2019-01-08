using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
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
               
            var splitClasses = new ComplexFactor()
            {
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Złożoność cyklomatyczna",
                        CurrentValue = measure.Value[SonarQubeConenction.Complexity] / 10,
                        IdealValue = 10
                    },
                    new SimpleFactor()
                    {
                        Name = "Duplikacja kodu",
                        CurrentValue = measure.Value[SonarQubeConenction.DuplicatedLinesDensity],
                        Kind = ValueType.Percentage,
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość linii na klasę",
                        CurrentValue = measure.Value[SonarQubeConenction.Lines] / measure.Value[SonarQubeConenction.Classes],
                        IdealValue = 330
                    }
                },
            };
            var futureDevelopment = new ComplexFactor()
            {
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Złożoność cyklomatyczna",
                        CurrentValue = measure.Value[SonarQubeConenction.Complexity] / 10,
                        IdealValue = 10
                    },
                    new SimpleFactor()
                    {
                        Name = "Zapachy kodu",
                        CurrentValue = measure.Value[SonarQubeConenction.CodeSmells]  / 10,
                        IdealValue = 30
                    },
                    new SimpleFactor()
                    {
                        Name = "Podatności",
                        CurrentValue = measure.Value[SonarQubeConenction.Vulnerabilities],
                        IdealValue = 10
                    }
                },
            };

            var simplicityOfCode = new ComplexFactor()
            {
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Ilość linii kodu na klasę",
                        CurrentValue = measure.Value[SonarQubeConenction.Lines] / measure.Value[SonarQubeConenction.Classes],
                        IdealValue = 330
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość linii kodu na metodę",
                        CurrentValue = measure.Value[SonarQubeConenction.Lines] / measure.Value[SonarQubeConenction.Functions],
                        IdealValue = 25
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość metod publicznych na klasę",
                        CurrentValue = measure.Value[SonarQubeConenction.Functions] / measure.Value[SonarQubeConenction.Classes],
                        IdealValue = 12
                    }
                }
            };
            NeedToSplitClasses.ComplexFactors.Add(splitClasses);
            ProblematicOfFutureDevelopment.ComplexFactors.Add(futureDevelopment);
            SimplicityOfCode.ComplexFactors.Add(simplicityOfCode);
            }
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
            var sqConnection = new SonarQubeConenction();
            var data = sqConnection.Ask();
            boo.AddDataFromSonarQube(data);

            InitializeComponent();
            DataContext = boo;
        }
    }
}
