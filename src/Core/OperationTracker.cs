using System;
using System.Threading;

namespace CCSWE
{
    /// <summary>
    /// A helper class to track the number of operations executing.
    /// </summary>
    public class OperationTracker
    {
        #region Constructor
        /// <summary>
        /// Create a new <see cref="OperationTracker"/>.
        /// </summary>
        /// <param name="setTargetPropertyAction">An <see cref="Action"/> used to set a target property when the <c>IsOperationRunning</c> property changes state.</param>
        public OperationTracker(Action<bool> setTargetPropertyAction = null)
        {
            _setTargetPropertyAction = setTargetPropertyAction;
        }
        #endregion

        #region Private Fields
        private readonly object _lock = new object();
        private long _operationsRunning;
        private readonly Action<bool> _setTargetPropertyAction;
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns <c>true</c> if an <c>OperationsRunning</c> is greater than zero.
        /// </summary>
        public bool IsOperationRunning => Interlocked.Read(ref _operationsRunning) > 0;

        /// <summary>
        /// Returns the number of operations currently running.
        /// </summary>
        public long OperationsRunning => Interlocked.Read(ref _operationsRunning);
        #endregion

        #region Private Methods
        private void OnSetTargetProperty()
        {
            _setTargetPropertyAction?.Invoke(IsOperationRunning);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Increment <c>OperationsRunning</c>.
        /// </summary>
        public void BeginOperation()
        {
            lock (_lock)
            {
                Interlocked.Increment(ref _operationsRunning);
                OnSetTargetProperty();
            }
        }

        /// <summary>
        /// Decrement <c>OperationsRunning</c>.
        /// </summary>
        public void EndOperation()
        {
            lock (_lock)
            {
                if (Interlocked.Read(ref _operationsRunning) <= 0)
                {
                    return;
                }

                Interlocked.Decrement(ref _operationsRunning);
                OnSetTargetProperty();
            }
        }
        #endregion
    }
}
