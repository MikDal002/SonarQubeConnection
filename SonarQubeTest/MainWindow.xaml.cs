using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SonarQubeTest
{
    public partial class TopLevel
    {
        [JsonProperty("component")]
        public Component Component { get; set; }
    }
    public partial class Component
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("qualifier")]
        public string Qualifier { get; set; }

        [JsonProperty("measures")]
        public List<Measure> Measures { get; set; }
    }

    public partial class Measure
    {
        [JsonProperty("metric")]
        public string Metric { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("periods")]
        public List<Period> Periods { get; set; }

        [JsonProperty("bestValue", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BestValue { get; set; }
    }

    public partial class Period
    {
        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("bestValue", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BestValue { get; set; }
    }
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    public class SonarQubeConenction
    {
        private string _baseAddress = @"http://localhost:9000/api/measures/component";
        private string _urlParams = $"?metricKeys={Vulnerabilities},{Lines},{Statements},{DuplicatedLinesDensity},{Complexity},{Functions},{Classes},{CodeSmells}&componentId=AWdwAlOm7p44trtMNcIB";

        public static string CodeSmells => "code_smells";
        public static string Classes => "classes";
        public static string Functions => "functions";
        public static string Complexity => "complexity";
        public static string DuplicatedLinesDensity => "duplicated_lines_density";
        public static string Statements => "statements";
        public static string Lines => "lines";
        public static string Vulnerabilities => "vulnerabilities";


        public Dictionary<string, double> Ask()
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_baseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/JSON"));

                HttpResponseMessage response = client.GetAsync(_urlParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync().Result;
                    var component = JsonConvert.DeserializeObject<TopLevel>(dataObjects);
                    return component.Component.Measures.ToDictionary(d => d.Metric, e => Convert.ToDouble(e.Value));
                }
                else
                {
                    throw new SonarQubeConnectionException("Serwer zwrócił odpowiedź o kodzie: " + response.StatusCode);
                }
            }
            catch (HttpRequestException exce)
            {
                throw new SonarQubeConnectionException("Nie udało sie nawiązać połączenia z SonarQube", exce);
            }
        }
    }

    public class SonarQubeConnectionException : Exception
    {
        public SonarQubeConnectionException()
        {
        }

        public SonarQubeConnectionException(string message) : base(message)
        {
        }

        public SonarQubeConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SonarQubeConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

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

        public void AddDataFromSonarQube(Dictionary<string, double> data)
        {
            var splitClasses = new ComplexFactor()
            {
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Złożoność cyklomatyczna",
                        CurrentValue = data[SonarQubeConenction.Complexity],
                        IdealValue = 1
                    },
                    new SimpleFactor()
                    {
                        Name = "Duplikacja kodu",
                        CurrentValue = data[SonarQubeConenction.DuplicatedLinesDensity],
                        IdealValue = 0.1,
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość linii na klasę",
                        CurrentValue = data[SonarQubeConenction.Lines] / data[SonarQubeConenction.Classes],
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
                        CurrentValue = data[SonarQubeConenction.Complexity],
                        IdealValue = 1
                    },
                    new SimpleFactor()
                    {
                        Name = "Zapachy kodu",
                        CurrentValue = data[SonarQubeConenction.CodeSmells],
                        IdealValue = 1,
                    },
                    new SimpleFactor()
                    {
                        Name = "Podatności",
                        CurrentValue = data[SonarQubeConenction.Vulnerabilities],
                        IdealValue = 1
                    }
                },
            };

            var simplicityOfCode = new ComplexFactor()
            {
                SimpleFactors = new List<SimpleFactor>()
                {
                    new SimpleFactor()
                    {
                        Name = "Ilość linii kodu",
                        CurrentValue = data[SonarQubeConenction.Lines],
                        IdealValue = 330
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość linii kodu na metodę",
                        CurrentValue = data[SonarQubeConenction.Lines] / data[SonarQubeConenction.Functions],
                        IdealValue = 25
                    },
                    new SimpleFactor()
                    {
                        Name = "Ilość metod publicznych na klasę",
                        CurrentValue = data[SonarQubeConenction.Functions] / data[SonarQubeConenction.Classes],
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
            var sqConnection = new SonarQubeConenction();
            var data = sqConnection.Ask();
            boo.AddDataFromSonarQube(data);

            InitializeComponent();
            DataContext = boo;
        }
    }
}
