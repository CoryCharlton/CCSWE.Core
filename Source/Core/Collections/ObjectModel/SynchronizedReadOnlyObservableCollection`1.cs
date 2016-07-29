using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace CCSWE.Collections.ObjectModel
{
    //TODO: SynchronizedReadOnlyObservableCollection<T> - Add xmldoc
    //TODO: SynchronizedReadOnlyObservableCollection<T> - Clean this up...
    public class SynchronizedReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructor
        public SynchronizedReadOnlyObservableCollection(SynchronizedObservableCollection<T> list): base((IList<T>)list)
        {
            _context = SynchronizationContext.Current;

            ((INotifyCollectionChanged)Items).CollectionChanged += HandleCollectionChanged;
            ((INotifyPropertyChanged)Items).PropertyChanged += HandlePropertyChanged;
        }
        #endregion

        #region Public Events
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Fields
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
        #endregion

        #region Protected Methods
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
