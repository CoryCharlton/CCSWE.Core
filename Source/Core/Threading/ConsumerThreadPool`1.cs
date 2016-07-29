using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CCSWE.Threading
{
    /// <summary>
    /// Provides a specialized thread pool to process items from a <see cref="BlockingCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ConsumerThreadPool<T> : IDisposable
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerThreadPool{T}"/> class specifying the number of consumer threads and the provided function to process items.
        /// </summary>
        /// <param name="consumersThreads">The number of consumber threads to create.</param>
        /// <param name="processItem">The function responsible for processing items added.</param>
        /// <param name="threadPriority">The <see cref="ThreadPriority"/> of the consumer threads</param>
        public ConsumerThreadPool(int consumersThreads, Func<T, bool> processItem, ThreadPriority threadPriority = ThreadPriority.BelowNormal) : this(consumersThreads, processItem, CancellationToken.None, threadPriority)
        {
            // Empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerThreadPool{T}"/> class specifying the number of consumer threads and the provided function to process items.
        /// </summary>
        /// <param name="consumersThreads">The number of consumber threads to create.</param>
        /// <param name="processItem">The function responsible for processing items added.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to signal that processing should stop</param>
        /// <param name="threadPriority">The <see cref="ThreadPriority"/> of the consumer threads</param>
        public ConsumerThreadPool(int consumersThreads, Func<T, bool> processItem, CancellationToken cancellationToken, ThreadPriority threadPriority = ThreadPriority.BelowNormal)
        {
            if (consumersThreads <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(consumersThreads), consumersThreads, "'consumerThreads' must be greater than zero");
            }

            if (processItem == null)
            {
                throw new ArgumentNullException(nameof(processItem));
            }

            _cancellationToken = cancellationToken;
            _items = new BlockingCollection<T>(new ConcurrentQueue<T>());
            _processItem = processItem;

            for (var i = 0; i < consumersThreads; i++)
            {
                var consumerThread = new Thread(ConsumerThread) { IsBackground = true, Priority = threadPriority };
                consumerThread.Start();
            }
        }
        #endregion

        #region Private Fields
        private readonly CancellationToken _cancellationToken;
        private readonly OperationTracker _consumerThreadTracker = new OperationTracker();
        private bool _isDisposed;
        private readonly BlockingCollection<T> _items;
        private long _itemsFailed;
        private long _itemsProcessed;
        private long _itemsSuccessful;
        private readonly Func<T, bool> _processItem;
        private long _totalItems;
        #endregion

        #region Public Properties
        /// <summary>
        /// Get the number of items that have failed processing.
        /// </summary>
        public long ItemsFailed => Interlocked.Read(ref _itemsFailed);

        /// <summary>
        /// Gets the total number of items the have been processed.
        /// </summary>
        public long ItemsProcessed => Interlocked.Read(ref _itemsProcessed);

        /// <summary>
        /// Gets the number of items that have successfully processed.
        /// </summary>
        public long ItemsSuccessful => Interlocked.Read(ref _itemsSuccessful);

        /// <summary>
        /// Gets whether this <see cref="ConsumerThreadPool{T}"/> has been cancelled.
        /// </summary>
        public bool IsCancelled => _cancellationToken.IsCancellationRequested;

        /// <summary>
        /// Gets whether this <see cref="ConsumerThreadPool{T}"/> has been marked as complete for adding and is empty.
        /// </summary>
        public bool IsCompleted => _items.IsCompleted && !_consumerThreadTracker.IsOperationRunning;

        /// <summary>
        /// Gets whether this <see cref="ConsumerThreadPool{T}"/> is continuing to accept items and the <see cref="Progress"/> value is accurate.
        /// </summary>
        public bool IsIndeterminate => !_items.IsAddingCompleted;

        /// <summary>
        /// Gets the progress of this <see cref="ConsumerThreadPool{T}"/>
        /// </summary>
        public double Progress { get; private set; }

        /// <summary>
        /// Gets the number of items that have been added. 
        /// </summary>
        public long TotalItems => Interlocked.Read(ref _totalItems);
        #endregion

        #region Private Methods
        private void ConsumerThread()
        {
            _consumerThreadTracker.BeginOperation();

            while (!_items.IsCompleted)
            {
                try
                {
                    T item;

                    try
                    {
                        item = _items.Take(_cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }

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
                        var progress = ((double)itemsProcessed / totalItems) * 100.0;
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

                    //TODO: ConsumerThreadPool<T> - Raise Progress property changed?
                }
                catch (InvalidOperationException)
                {
                    // Move along nothing to see here...
                }
            }

            _consumerThreadTracker.EndOperation();
            //TODO: ConsumerThreadPool<T> - Raise IsCompleted property changed?

#if DEBUG
            Debug.WriteLine("ConsumerThread exiting... Active threads: " + _consumerThreadTracker.OperationsRunning);
#endif
        }
        #endregion

        #region Protected Methods
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            CompleteAdding();

            //TODO: ConsumerThreadPool<T> - Wait for completion? Cancel?
            _items.Dispose();
            _isDisposed = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the item to the <see cref="ConsumerThreadPool{T}"/>.
        /// </summary>
        /// <param name="item">Adds the item to the <see cref="ConsumerThreadPool{T}"/>. The item cannot be a null reference.</param>
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "'item' cannot be null");
            }

            try
            {
                _items.Add(item, _cancellationToken);

                Interlocked.Increment(ref _totalItems);
            }
            catch (OperationCanceledException)
            {
                CompleteAdding();
            }
        }

        /// <summary>
        /// Marks the <see cref="ConsumerThreadPool{T}"/> instance as not accepting any more additions.
        /// </summary>
        public void CompleteAdding()
        {
            _items.CompleteAdding();

            //TODO: ConsumerThreadPool<T> - Raise IsAddingComplete and IsIndeterminate property changed?
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="ConsumerThreadPool{T}"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Spins until all items have been processed.
        /// </summary>
        /// <param name="ignoreIsAddingCompleted">Set this to true if you want to wait in this thread and will call <see cref="CompleteAdding"/> in another thread.</param>
        /// <returns></returns>
        public async Task WaitForCompletion(bool ignoreIsAddingCompleted = false)
        {
            if (!ignoreIsAddingCompleted && !_items.IsAddingCompleted)
            {
                throw new InvalidOperationException("Adding is not completed");
            }

            while (!IsCancelled && !IsCompleted)
            {
                await Task.Delay(100, _cancellationToken);
            }
        }
        #endregion
    }
}
