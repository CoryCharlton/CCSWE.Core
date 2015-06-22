using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using GalaSoft.MvvmLight.CommandWpf;

namespace CCSWE.Windows.Input
{
    //TODO: Move this to another project so the main library doesn't have a dependency on MVVM light
    public class DependentRelayCommand : RelayCommand
    {

        private readonly List<string> _dependentPropertyNames;

        public DependentRelayCommand(Action execute, Func<bool> canExecute,
                                     INotifyPropertyChanged target,
                                     params string[] dependentPropertyNames)
            : base(execute, canExecute)
        {
            _dependentPropertyNames = new List<string>(dependentPropertyNames);

            target.PropertyChanged += TargetPropertyChanged;
        }

        public DependentRelayCommand(Action execute, Func<bool> canExecute,
                                     INotifyPropertyChanged target,
                                     params Expression<Func<object>>[] dependentPropertyExpressions)
            : base(execute, canExecute)
        {
            _dependentPropertyNames = new List<string>();
            foreach (var body in dependentPropertyExpressions.Select(expression => expression.Body))
            {
                var expression = body as MemberExpression;
                if (expression != null)
                {
                    _dependentPropertyNames.Add(expression.Member.Name);
                }
                else
                {
                    var unaryExpression = body as UnaryExpression;
                    if (unaryExpression != null)
                    {
                        _dependentPropertyNames.Add(((MemberExpression)unaryExpression.Operand).Member.Name);
                    }
                }
            }

            target.PropertyChanged += TargetPropertyChanged;

        }

        private void TargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_dependentPropertyNames.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }

    }
}
