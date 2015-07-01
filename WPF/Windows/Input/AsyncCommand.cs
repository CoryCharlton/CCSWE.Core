using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CCSWE.Threading.Tasks;

// Based on the article `Patterns for Asynchronous MVVM Applications: Commands` by Stephen Cleary (https://msdn.microsoft.com/en-us/magazine/dn630647.aspx)
namespace CCSWE.Windows.Input
{
    /// <summary>
    /// An <see cref="ICommand"/> implementation around a <see cref="Task"/> that extends the CanExecute() method to check whether or not the <see cref="Task"/> is already executing
    /// </summary>
    public class AsyncCommand : AsyncCommandBase
    {
        #region Constructor
        public AsyncCommand(Func<Task> execute): this(execute, null)
        {

        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        #endregion

        #region Private Fields
        //TODO: AsyncCommand - MVVM Light implements a WeakAction and WeakFunc. Need to look into wether or not something like that is necessary
        private readonly Func<bool> _canExecute;
        //private readonly List<string> _dependentPropertyNames;
        private readonly Func<Task> _execute;
        private NotifyTaskCompletion _execution;
        #endregion

        #region Public Properties
        public NotifyTaskCompletion Execution
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
        /*
        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_dependentPropertyNames.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }
        */
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

            return _canExecute();
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            Execution = new NotifyTaskCompletion(_execute());
            RaiseCanExecuteChanged();

            await Execution.TaskCompletion;

            RaiseCanExecuteChanged();
        }
        #endregion
    }
}