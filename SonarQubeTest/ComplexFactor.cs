using System;
using System.Collections.Generic;
using System.Linq;

namespace SonarQubeTest
{
    public enum ComplexFactorQuality
    {
        Excelent,
        Good,
        Average,
        Poor,
        Unsatisfactory,
    }
    public class ComplexFactor
    {
        public DateTimeOffset Time { get; set; }
        public ComplexFactorQuality QualityCategory
        {
            get
            {
                if (Quality >= 0.95) return ComplexFactorQuality.Excelent;
                if (Quality >= 0.90) return ComplexFactorQuality.Good;
                if (Quality >= 0.80) return ComplexFactorQuality.Average;
                if (Quality >= 0.70) return ComplexFactorQuality.Poor;
                return ComplexFactorQuality.Unsatisfactory;
            }
        }
        public double Quality => SimpleFactors.Select(d => d.Quality).Average();

        public List<SimpleFactor> SimpleFactors { get; set; }
    }
}