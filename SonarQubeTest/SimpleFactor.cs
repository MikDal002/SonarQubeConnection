using System;

namespace SonarQubeTest
{
    public class SimpleFactor
    {
        public string Name { get; set; }
        public double CurrentValue { get; set; }
        public double IdealValue { get; set; }
        public double Quality => (1 - Math.Abs(CurrentValue / IdealValue - 1));
    }
}