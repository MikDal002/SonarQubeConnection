using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using LiveCharts;
using LiveCharts.Defaults;

namespace SonarQubeTest
{
    public class ComplexFactorVM : INotifyPropertyChanged
    {
        public IChartValues ChartValues { get; set; } = new ChartValues<ObservableValue>();
        public ObservableCollection<ComplexFactor> ComplexFactors { get; set; } = new ObservableCollection<ComplexFactor>();
        public ComplexFactor Current => ComplexFactors.LastOrDefault();

        public string Name { get; set; }
        public string Description { get; set; }

        public ComplexFactorVM()
        {
            var changed = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(h => ComplexFactors.CollectionChanged += h, h => ComplexFactors.CollectionChanged -= h);
            changed.Subscribe(d =>
            {
                if (d.EventArgs.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var newPoint in d.EventArgs.NewItems)
                        ChartValues.Add(new ObservableValue((newPoint as ComplexFactor).Quality));
                }
            });
            changed.Subscribe(d => OnPropertyChanged(nameof(ComplexFactors), nameof(Current)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                OnPropertyChanged(name);
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}