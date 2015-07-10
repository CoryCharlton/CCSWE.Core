using System;
using System.ComponentModel;
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
    /// <typeparam name="TResult">The result type of the target <see cref="Task"/></typeparam>
    public sealed class NotifyTaskCompletion<TResult> : NotifyTaskCompletion
    {
        #region Constructor
        // ReSharper disable once SuggestBaseTypeForParameter
        public NotifyTaskCompletion(Task<TResult> task) : base(task)
        {

        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the result value of this <see cref="Task{TResult}"/>.
        /// </summary>
        public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? ((Task<TResult>)Task).Result : default(TResult); } }
        #endregion
    }
}