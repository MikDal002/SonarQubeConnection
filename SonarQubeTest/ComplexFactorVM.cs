using System.Collections.Generic;
using System.Linq;
using LiveCharts;
using LiveCharts.Defaults;

namespace SonarQubeTest
{
    public class ComplexFactorVM
    {
        public IChartValues ChartValues =>
            new ChartValues<ObservableValue>(ComplexFactors.Select(d => new ObservableValue(d.Quality)));
        public List<ComplexFactor> ComplexFactors { get; set; }
        public ComplexFactor Current => ComplexFactors.Last();

        public string Name { get; set; }
        public string Description { get; set; }
    }
}