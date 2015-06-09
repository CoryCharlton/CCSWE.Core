using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CCSWE.Threading
{
    public class ConsumerThreadPool<T>: IDisposable
    {
        #region Constructor
        public ConsumerThreadPool(int consumersThreads, Func<T, bool> processItem)
        {
            _items = new BlockingCollection<T>(new ConcurrentQueue<T>());
            _processItem = processItem;

            for (var i = 0; i < consumersThreads; i++)
            {
                var workerThread = new Thread(ConsumerThread) { IsBackground = true, Priority = ThreadPriority.BelowNormal };
                workerThread.Start();
            }
        }
        #endregion

        #region Private Fields
        private readonly OperationTracker _consumerThreadTracker = new OperationTracker();
        private readonly BlockingCollection<T> _items;
        private long _itemsFailed;
        private long _itemsProcessed;
        private long _itemsSuccessful;
        private readonly Func<T, bool> _processItem;
        private long _totalItems;
        #endregion

        #region Public Properties
        public long ItemsFailed { get { return Interlocked.Read(ref _itemsFailed); } }
        public long ItemsProcessed { get { return Interlocked.Read(ref _itemsProcessed); } }
        public long ItemsSuccessful { get { return Interlocked.Read(ref _itemsSuccessful); } }
        public bool IsCompleted { get { return _items.IsCompleted && !_consumerThreadTracker.IsOperationRunning; } }
        public bool IsIndeterminate { get { return !_items.IsAddingCompleted; } }
        public double Progress { get; protected set; }
        public long TotalItems { get { return Interlocked.Read(ref _totalItems); } }
        #endregion

        #region Private Methods
        private void ConsumerThread()
        {
            _consumerThreadTracker.BeginOperation();

            while (!_items.IsCompleted)
            {
                try
                {
                    var item = _items.Take();
                    if (_processItem(item))
                    {
                        Interlocked.Increment(ref _itemsSuccessful);
                    }
                    else
                    {
                        Interlocked.Increment(ref _itemsFailed);
                    }

                    var itemsProcessed = Interlocked.Increment(ref _itemsProcessed);
                    var totalItems = Interlocked.Read(ref _totalItems);

                    if (itemsProcessed > 0)
                    {
                        var progress = ((double) itemsProcessed/totalItems)*100.0;
                        if (progress > 100.0)
                        {
                            progress = 100;
                        }

                        Progress = progress;
                    }
                    else
                    {
                        Progress = 0;
                    }

                    //TODO: Raise Progress property changed?
                }
                catch (InvalidOperationException exception)
                {
                    // Move along nothing to see here...
                }
            }

            _consumerThreadTracker.EndOperation();
            //TODO: Raise IsCompleted property changed?

            Debug.WriteLine("ConsumerThread exiting... " + _consumerThreadTracker.OperationsRunning);
        }
        #endregion

        #region Public Methods
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "'item' cannot be null");
            }

            _items.Add(item);

            Interlocked.Increment(ref _totalItems);
        }

        public void CompleteAdding()
        {
            _items.CompleteAdding();

            //TODO: Raise IsAddingComplete and IsIndeterminate property changed?
        }

        public void Dispose()
        {
            CompleteAdding();

            //TODO: Wait for completion? Cancel?
            _items.Dispose();
        }

        public async Task WaitForCompletion()
        {
            while (!IsCompleted)
            {
                await Task.Delay(100);
            }
        }
        #endregion

    }
}
