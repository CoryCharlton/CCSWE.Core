using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CCSWE.Collections.ObjectModel
{
    /// <summary>Represents a thread-safe dynamic data collection that provides notifications when items get added, removed, or when the whole collection is refreshed.</summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [Serializable]
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class SynchronizedReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructor
        /// <summary>Initializes a new instance of the <see cref="SynchronizedReadOnlyObservableCollection{T}" /> class that is a read-only wrapper around the specified <see cref="SynchronizedObservableCollection{T}"/>.</summary>
        /// <param name="collection">The collection to wrap.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="collection" /> is null.</exception>
        public SynchronizedReadOnlyObservableCollection(SynchronizedObservableCollection<T> collection): this(collection, collection?.Context)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SynchronizedReadOnlyObservableCollection{T}" /> class that is a read-only wrapper around the specified <see cref="SynchronizedObservableCollection{T}"/>.</summary>
        /// <param name="collection">The collection to wrap.</param>
        /// <param name="context">The context used for event invokation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter cannot be null.</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> is null.</exception>
        public SynchronizedReadOnlyObservableCollection(SynchronizedObservableCollection<T> collection, SynchronizationContext context) : base(collection)
        {
            Ensure.IsNotNull(nameof(context), context);
            Ensure.IsNotNull(nameof(collection), collection);

            _context = context;

            ((INotifyCollectionChanged) Items).CollectionChanged += HandleCollectionChanged;
            ((INotifyPropertyChanged) Items).PropertyChanged += HandlePropertyChanged;
        }

        #endregion

        #region Public Events
        /// <summary>Occurs when an item is added, removed, changed, moved, or the entire collection is refreshed.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Fields
        [NonSerialized]
        private readonly SynchronizationContext _context;
        #endregion

        #region Private Methods
        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var collectionChanged = CollectionChanged;
            if (collectionChanged == null)
            {
                return;
            }

            _context.Send(state => collectionChanged(this, e), null);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }

            _context.Send(state => propertyChanged(this, e), null);
        }
        #endregion
    }
}
