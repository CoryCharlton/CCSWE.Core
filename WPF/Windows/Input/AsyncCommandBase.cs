using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CCSWE.Windows.Input
{
    /// <summary>
    /// A base class used to implement <see cref="ICommand"/> around a <see cref="Task"/>
    /// </summary>
    /// <remarks>Based on the article `Patterns for Asynchronous MVVM Applications: Commands` by Stephen Cleary (https://msdn.microsoft.com/en-us/magazine/dn630647.aspx)</remarks>
    public abstract class AsyncCommandBase : IAsyncCommand, INotifyPropertyChanged
    {
        #region Public Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Protected Methods
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var propertyChangedEventHandler = PropertyChanged;
            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Public Methods
        public abstract bool CanExecute(object parameter);

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            await ExecuteAsync(parameter);
        }

        public abstract Task ExecuteAsync(object parameter);

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
        #endregion
    }
}