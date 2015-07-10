using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

// Based on the article `Patterns for Asynchronous MVVM Applications: Data Binding` by Stephen Cleary (https://msdn.microsoft.com/magazine/dn605875)
// Looks like Stephen also included this in his AsyncEx library which you might want to check out https://github.com/StephenCleary/AsyncEx
namespace CCSWE.Threading.Tasks
{
    /// <summary>
    /// A helper class that allows data binding to properties related to the status of a <see cref="Task"/>
    /// </summary>
    /// <remarks>
    /// Based on the article `Patterns for Asynchronous MVVM Applications: Data Binding` by Stephen Cleary (https://msdn.microsoft.com/magazine/dn605875)
    /// Looks like Stephen also included this in his AsyncEx library which you might want to check out https://github.com/StephenCleary/AsyncEx
    /// </remarks>
    public class NotifyTaskCompletion : INotifyPropertyChanged
    {
        #region Constructor
        public NotifyTaskCompletion(Task task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }
        #endregion

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Properties
        public string ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }
        public AggregateException Exception { get { return Task.Exception; } }
        public Exception InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }
        public bool IsCanceled { get { return Task.IsCanceled; } }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public bool IsFaulted { get { return Task.IsFaulted; } }
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        /// <summary>
        /// Gets the <see cref="TaskStatus"/> of the target <see cref="Task"/>
        /// </summary>
        public TaskStatus Status { get { return Task.Status; } }

        /// <summary>
        /// Gets the target <see cref="Task"/>
        /// </summary>
        //TODO: Hide this one?
        public Task Task { get; private set; }

        /// <summary>
        /// Gets a <see cref="Task"/> used to await the target <see cref="Task"/> and notify status when completed
        /// </summary>
        public Task TaskCompletion { get; private set; }
        #endregion

        #region Private Methods
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                if (!task.IsCompleted)
                {
                    await task;
                }
            }
            //catch
            catch (Exception exception)
            {
                Debug.WriteLine("NotifyTaskCompletion.WatchTaskAsync() - " + exception);
                // Move along nothing to see here
            }

            RaisePropertyChanged("Status");
            RaisePropertyChanged("IsCompleted");

            if (task.IsCanceled)
            {
                RaisePropertyChanged("IsCanceled");
            }
            else if (task.IsFaulted)
            {
                RaisePropertyChanged("ErrorMessage");
                RaisePropertyChanged("Exception");
                RaisePropertyChanged("InnerException");
                RaisePropertyChanged("IsFaulted");
            }
            else
            {
                RaisePropertyChanged("IsSuccessfullyCompleted");
                RaisePropertyChanged("Result");
            }
        }
        #endregion

        #region Protected Methods
        protected void RaisePropertyChanged(string propertyName)
        {
            var propertyChangedHandler = PropertyChanged;
            if (propertyChangedHandler != null)
            {
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }        
        #endregion
    }
}