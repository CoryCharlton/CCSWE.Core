using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using CCSWE.Collections.ObjectModel;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable ObjectCreationAsStatement
namespace CCSWE.Core.UnitTests.Collections.ObjectModel
{
    [TestFixture]
    public class SynchronizedReadOnlyObservableCollectionTest
    {
        [TestFixture]
        public class When_Constructor_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_collection_is_not_null()
            {
                var collection = new SynchronizedReadOnlyObservableCollection<string>(new SynchronizedObservableCollection<string>{ "1", "2", "3" });

                Assert.That(collection.Count, Is.EqualTo(3));
            }

            [Test]
            public void It_does_not_throw_exception_when_collection_and_context_are_not_null()
            {
                var collection = new SynchronizedReadOnlyObservableCollection<string>(new SynchronizedObservableCollection<string> { "1", "2", "3" }, new SynchronizationContext());

                Assert.That(collection.Count, Is.EqualTo(3));
            }

            [Test]
            public void It_throws_exception_when_collection_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new SynchronizedReadOnlyObservableCollection<string>(null));
                Assert.Throws<ArgumentNullException>(() => new SynchronizedReadOnlyObservableCollection<string>(null, new SynchronizationContext()));
            }

            [Test]
            public void It_throws_exception_when_context_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new SynchronizedReadOnlyObservableCollection<string>(new SynchronizedObservableCollection<string>(), null));
            }
        }

        [TestFixture]
        public class When_SynchronizedObservableCollection_CollectionChanged_is_invoked
        {
            [Test]
            public void It_invokes_CollectionChanged()
            {
                var collection = new SynchronizedObservableCollection<string>(new List<string> { "1", "2", "3" });
                var readonlyCollection = new SynchronizedReadOnlyObservableCollection<string>(collection);
                NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;

                readonlyCollection.CollectionChanged += (sender, args) => { collectionChangedEventArgs = args; };
                collection.Add("4");

                Assert.That(collectionChangedEventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                Assert.That(collectionChangedEventArgs.NewItems[0], Is.EqualTo("4"));
                Assert.That(collectionChangedEventArgs.NewStartingIndex, Is.EqualTo(3));
            }
        }

        [TestFixture]
        public class When_SynchronizedObservableCollection_PropertyChanged_is_invoked
        {
            [Test]
            public void It_invokes_PropertyChanged()
            {
                var collection = new SynchronizedObservableCollection<string>();
                var readonlyCollection = new SynchronizedReadOnlyObservableCollection<string>(collection);
                var propertyChangedEventArgs = new List<PropertyChangedEventArgs>();

                readonlyCollection.PropertyChanged += (sender, args) => { propertyChangedEventArgs.Add(args); };
                collection.Add("4");

                Assert.That(propertyChangedEventArgs.Count, Is.EqualTo(2));
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Count")), Is.True);
                Assert.That(propertyChangedEventArgs.Any(p => p.PropertyName.Equals("Item[]")), Is.True);
            }
        }
    }
}
