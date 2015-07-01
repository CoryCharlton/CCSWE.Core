using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CCSWE.Threading.Tasks;

//TODO: CancellableAsyncCommand<TResult> - This needs work to bring in line with AsyncCommand. Checking in because it works for now and is a reasonable start...
// Based on the article `Patterns for Asynchronous MVVM Applications: Commands` by Stephen Cleary (https://msdn.microsoft.com/en-us/magazine/dn630647.aspx)
namespace CCSWE.Windows.Input
{
    /// <summary>
    /// An <see cref="ICommand"/> implementation around a <see cref="Task"/> that extends the CanExecute() method to check whether or not the <see cref="Task"/> is already executing and provides a child <see cref="CancelCommand"/> to allow cancellation of the <see cref="Task"/>
    /// </summary>
    /// <typeparam name="TResult">The result type of the target <see cref="Task"/></typeparam>
    public class CancellableAsyncCommand<TResult> : AsyncCommandBase
    {
        #region Constructor
        public CancellableAsyncCommand(Func<CancellationToken, Task<TResult>> execute): this(execute, null)
        {
        }

        //TODO: Implement _canExecute in AsyncCommand<TResult>
        public CancellableAsyncCommand(Func<CancellationToken, Task<TResult>> execute, Func<bool> canExecute)
        {
            _cancelCommand = new CancelAsyncCommand();
            _canExecute = canExecute;
            _execute = execute;
        }

        public CancellableAsyncCommand(Func<CancellationToken, Task<TResult>> execute, Func<bool> canExecute, INotifyPropertyChanged target, params Expression<Func<object>>[] dependentPropertyExpressions)
        {
            _cancelCommand = new CancelAsyncCommand();
            _canExecute = canExecute;
            _execute = execute;

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

            target.PropertyChanged += OnTargetPropertyChanged;
        }
        #endregion

        #region Private Fields
        private readonly CancelAsyncCommand _cancelCommand;
        //TODO: AsyncCommand<TResult> - MVVM Light implements a WeakAction and WeakFunc. Need to look into wether or not something like that is necessary
        private readonly Func<bool> _canExecute;
        private readonly List<string> _dependentPropertyNames;
        private readonly Func<CancellationToken, Task<TResult>> _execute;
        private NotifyTaskCompletion<TResult> _execution;
        #endregion

        #region Public Properties
        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        public NotifyTaskCompletion<TResult> Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Private Methods
        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_dependentPropertyNames.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }        
        #endregion

        #region Public Methods
        public override bool CanExecute(object parameter)
        {
            // Check if already executing
            if (Execution != null && !Execution.IsCompleted)
            {
                return false;
            }

            // There is no function to check so I suppose we can execute :)
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute.Invoke();
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _cancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(_execute(_cancelCommand.Token));
            RaiseCanExecuteChanged();

            await Execution.TaskCompletion;

            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }
        #endregion

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;

            public CancellationToken Token
            {
                get { return _cts.Token; }
            }

            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested)
                    return;
                _cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return _commandExecuting && !_cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}