using System;

namespace SonarQubeTest
{
    public enum ValueType
    {
        Numeric,
        Percentage
    }
    public class SimpleFactor
    {
        public string Name { get; set; }
        public double CurrentValue { get; set; }
        public double IdealValue { get; set; }
        public ValueType Kind { get; set; } = ValueType.Numeric;
        public double Quality
        {
            get
            {
                if (Kind == ValueType.Numeric)
                {
                    var value = 1 - Math.Abs(CurrentValue / IdealValue - 1);
                    if (value < 0) return 0;
                    else if (value > 1) return 1;
                    return value;
                }
                else
                    return 1 - (CurrentValue / 100 );
            }
        }
    }
}