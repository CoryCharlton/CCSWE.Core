using System;
using System.Threading;

namespace CCSWE
{
    //TODO: OperationTracker - Add xmldoc
    public class OperationTracker
    {
        #region Constructor
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
        public bool IsOperationRunning => Interlocked.Read(ref _operationsRunning) > 0;

        public long OperationsRunning => Interlocked.Read(ref _operationsRunning);
        #endregion

        #region Private Methods
        private void OnSetTargetProperty()
        {
            _setTargetPropertyAction?.Invoke(IsOperationRunning);
        }
        #endregion

        #region Public Methods
        public void BeginOperation()
        {
            lock (_lock)
            {
                Interlocked.Increment(ref _operationsRunning);
                OnSetTargetProperty();
            }
        }

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
