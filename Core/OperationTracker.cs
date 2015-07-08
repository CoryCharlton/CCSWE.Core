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
        private readonly Action<bool> _setTargetPropertyAction;
        private readonly object _lock = new object();
        private long _operationsRunning;
        #endregion

        #region Public Properties
        public bool IsOperationRunning
        {
            get { return (Interlocked.Read(ref _operationsRunning) > 0); }
        }

        public long OperationsRunning
        {
            get { return Interlocked.Read(ref _operationsRunning); }
        }
        #endregion

        #region Private Methods
        private void OnSetTargetProperty()
        {
            var operationsRunning = Interlocked.Read(ref _operationsRunning);
            if (_setTargetPropertyAction != null)
            {
                _setTargetPropertyAction(operationsRunning > 0);
            }
        }
        #endregion

        #region Public Methods
        public void BeginOperation()
        {
            lock(_lock)
            {
                Interlocked.Increment(ref _operationsRunning);
                OnSetTargetProperty();
            }
        }

        public void EndOperation()
        {
            lock (_lock)
            {
                if (Interlocked.Read(ref _operationsRunning) > 0)
                {
                    Interlocked.Decrement(ref _operationsRunning);
                    OnSetTargetProperty();
                }
            }
        }
        #endregion
    }
}
