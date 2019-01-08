using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace SonarQubeTest
{
    public static class StaticMoc
    {
        static public ComplexFactorVM ViewModelMoc { get; set; } = new ComplexFactorVM()
        {

            Name = "Prostota kodu",
            Description = "Prostota bieżącego stanu projektu mówi o tym, czy klasy i metody mają odpowiednią wielkość.",
            ComplexFactors = new ObservableCollection<ComplexFactor>()
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
    /// Interaction logic for MaterialCard.xaml
    /// </summary>
    public partial class MaterialCard : UserControl, INotifyPropertyChanged
    {

        private double _lastLecture;
        private double _trend;

        public MaterialCard()
        {
            InitializeComponent();

            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8)
                    }
                }
            };
            _trend = 8;

#if NET40
            Task.Factory.StartNew(() =>
            {
                var r = new Random();
 
                Action action = delegate
                {
                    LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                    LastHourSeries[0].Values.RemoveAt(0);
                    SetLecture();
                };
 
                while (true)
                {
                    Thread.Sleep(500);
                    _trend += (r.NextDouble() > 0.3 ? 1 : -1) * r.Next(0, 5);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, action);
                }
            });
#endif
#if NET45
            Task.Run(() =>
            {
                var r = new Random();
                while (true)
                {
                    Thread.Sleep(500);
                    _trend += (r.NextDouble() > 0.3 ? 1 : -1)*r.Next(0, 5);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                        LastHourSeries[0].Values.RemoveAt(0);
                        SetLecture();
                    });
                }
            });
#endif

            DataContext = this;
        }

        public SeriesCollection LastHourSeries { get; set; }

        public double LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }

        private void SetLecture()
        {
            var target = ((ChartValues<ObservableValue>)LastHourSeries[0].Values).Last().Value;
            var step = (target - _lastLecture) / 4;
#if NET40
            Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });
#endif
#if NET45
            Task.Run(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });
#endif
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            //TimePowerChart.Update(true);
        }
    }
}