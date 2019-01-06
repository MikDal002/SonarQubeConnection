using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private string _urlParams = @"?metricKeys=vulnerabilities,lines,statements,duplicated_lines_density,complexity,functions,classes,code_smells&componentId=AWdwAlOm7p44trtMNcIB";
        public void Ask()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/JSON"));

            HttpResponseMessage response = client.GetAsync(_urlParams).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                var component = JsonConvert.DeserializeObject<TopLevel>(dataObjects);
            }
        }
    }

    public static class MainWindowVM
    {
        static public ComplexFactorVM NeedToSplitClasses = new ComplexFactorVM()
        {
            Name = "Potrzeba rozbicia klas",
            Description = "Paramtetr określający na ile klasy obecne w projekcie są przerośnięte lub nadmiernie skomplikowane i mogą w przyszłości utrudniać zrozumienie kodu oraz rozwój aplikacji.",
            ComplexFactors = new List<ComplexFactor>()
            {
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Złożoność cyklomatyczna",
                            CurrentValue = 0.9,
                            IdealValue = 1
                        },
                        new SimpleFactor()
                        {
                            Name = "Duplikacja kodu",
                            CurrentValue = 4,
                            IdealValue = 0.1,
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość linii na klasę",
                            CurrentValue = 430,
                            IdealValue = 330
                        }
                    },
                },
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Złożoność cyklomatyczna",
                            CurrentValue = 0.91,
                            IdealValue = 1
                        },
                        new SimpleFactor()
                        {
                            Name = "Duplikacja kodu",
                            CurrentValue = 3,
                            IdealValue = 0.1,
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość linii na klasę",
                            CurrentValue = 410,
                            IdealValue = 330
                        }
                    },
                },new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Złożoność cyklomatyczna",
                            CurrentValue = 0.95,
                            IdealValue = 1
                        },
                        new SimpleFactor()
                        {
                            Name = "Duplikacja kodu",
                            CurrentValue = 2,
                            IdealValue = 0.1,
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość linii na klasę",
                            CurrentValue = 380,
                            IdealValue = 330
                        }
                    },
                },
            }
        };
        static public ComplexFactorVM ProblematicOfFutureDevelopment = new ComplexFactorVM()
        {
            Name = "Problematyka dalszego rozwoju",
            Description = "Parametr ten okresla na ile problematyczny może być dalszy rozwoj aplikacji. Uwzglednione są wszelkie złe praktyki stosowane w kodzie oraz ilość zagnieżdżeń różnego rodzaju pętli i instrukcji warunkowych",
            ComplexFactors = new List<ComplexFactor>()
            {
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Złożoność cyklomatyczna",
                            CurrentValue = 0.9,
                            IdealValue = 1
                        },
                        new SimpleFactor()
                        {
                            Name = "Zapachy kodu",
                            CurrentValue = 10,
                            IdealValue = 1,
                        },
                        new SimpleFactor()
                        {
                            Name = "Podatności",
                            CurrentValue = 10,
                            IdealValue = 1
                        }
                    },
                },
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Złożoność cyklomatyczna",
                            CurrentValue = 0.91,
                            IdealValue = 1
                        },
                        new SimpleFactor()
                        {
                            Name = "Zapachy kodu",
                            CurrentValue = 8,
                            IdealValue = 1,
                        },
                        new SimpleFactor()
                        {
                            Name = "Podatności",
                            CurrentValue = 8,
                            IdealValue = 1
                        }
                    },
                },
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Złożoność cyklomatyczna",
                            CurrentValue = 0.95,
                            IdealValue = 1
                        },
                        new SimpleFactor()
                        {
                            Name = "Zapachy kodu",
                            CurrentValue = 5,
                            IdealValue = 1,
                        },
                        new SimpleFactor()
                        {
                            Name = "Podatności",
                            CurrentValue = 5,
                            IdealValue = 1
                        }
                    },
                }
            }
        };
        static public ComplexFactorVM SimplicityOfCode = new ComplexFactorVM()
        {

            Name = "Prostota kodu",
            Description = "Prostota bieżącego stanu projektu mówi o tym, czy klasy i metody mają odpowiednią wielkość.",
            ComplexFactors = new List<ComplexFactor>()
            {
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Ilość linii kodu",
                            CurrentValue = 430,
                            IdealValue = 330
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość linii kodu na metodę",
                            CurrentValue = 35,
                            IdealValue = 25
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość metod publicznych na klasę",
                            CurrentValue = 8,
                            IdealValue = 12
                        }
                    }
                },
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Ilość linii kodu",
                            CurrentValue = 420,
                            IdealValue = 330
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość linii kodu na metodę",
                            CurrentValue = 32,
                            IdealValue = 25
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość metod publicznych na klasę",
                            CurrentValue = 10,
                            IdealValue = 12
                        }
                    },
                },
                new ComplexFactor()
                {
                    SimpleFactors = new List<SimpleFactor>()
                    {
                        new SimpleFactor()
                        {
                            Name = "Ilość linii kodu",
                            CurrentValue = 410,
                            IdealValue = 330
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość linii kodu na metodę",
                            CurrentValue = 30,
                            IdealValue = 25
                        },
                        new SimpleFactor()
                        {
                            Name = "Ilość metod publicznych na klasę",
                            CurrentValue = 11,
                            IdealValue = 12
                        }
                    },
                },
            }


        };
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var foo = new SonarQubeConenction();
            foo.Ask();
        }
    }
}
