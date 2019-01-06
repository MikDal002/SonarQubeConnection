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

namespace SonarQubeTest
{
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
        static public  ComplexFactorVM SimplicityOfCode = new ComplexFactorVM()
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
        }
    }
}
