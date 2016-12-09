using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCSWE.Collections.Generic;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable ObjectCreationAsStatement

namespace CCSWE.Core.UnitTests.Collections.Generic
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class ThreadSafeQueueTest
    {
        [TestFixture]
        public class When_Clear_is_called
        {
            [Test]
            public void It_removes_all_items()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});

                Assert.That(queue.Count, Is.GreaterThan(0));
                queue.Clear();
                Assert.That(queue.Count, Is.EqualTo(0));
            }
        }

        [TestFixture]
        public class When_Constructor_is_called
        {
            [Test]
            public void It_does_not_throw_exception()
            {
                Assert.DoesNotThrow(() => new ThreadSafeQueue<string>());
            }

            [Test]
            public void It_does_not_throw_exception_when_capacity_is_in_range()
            {
                Assert.DoesNotThrow(() => new ThreadSafeQueue<string>(0));
            }

            [Test]
            public void It_does_not_throw_exception_when_collection_is_not_null()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});

                Assert.That(queue.Count, Is.EqualTo(3));
            }

            [Test]
            public void It_throws_exception_when_capacity_is_out_of_range()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new ThreadSafeQueue<string>(-1));
            }

            [Test]
            public void It_throws_exception_when_collection_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new ThreadSafeQueue<string>(null));
            }
        }

        [TestFixture]
        public class When_Contains_is_called
        {
            [Test]
            public void It_returns_false_if_queue_does_not_contain_item()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});

                Assert.That(queue.Contains("4"), Is.False);
            }

            [Test]
            public void It_returns_true_if_queue_contains_item()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});

                Assert.That(queue.Contains("3"), Is.True);
            }
        }

        [TestFixture]
        public class When_CopyTo_is_called
        {
            [Test]
            public void It_copies_items_to_array()
            {
                var array = new string[3];
                var queue = new ThreadSafeQueue<string>(new List<string> { "1", "2", "3" });

                Assert.DoesNotThrow(() => queue.CopyTo(array, 0));
                Assert.That(array.Length, Is.EqualTo(3));
                Assert.That(array[0], Is.EqualTo("1"));
                Assert.That(array[1], Is.EqualTo("2"));
                Assert.That(array[2], Is.EqualTo("3"));
            }

            [Test]
            public void It_throws_exception_when_arrayIndex_is_less_than_zero()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => queue.CopyTo(new string[1], -1));
            }

            [Test]
            public void It_throws_exception_when_arrayIndex_is_greater_than_or_equal_to_array_length()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentOutOfRangeException>(() => queue.CopyTo(new string[1], 1));
            }

            [Test]
            public void It_throws_exception_when_array_is_null()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentNullException>(() => queue.CopyTo(null, 0));
            }

            [Test]
            public void It_throws_exception_when_offset_is_less_than_count()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> { "1", "2", "3" });

                Assert.Throws<ArgumentException>(() => queue.CopyTo(new string[3], 1));
            }
        }

        [TestFixture]
        public class When_Dequeue_is_called
        {
            [Test]
            public void It_removes_first_item_from_the_queue()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});
                var item = queue.Dequeue();

                Assert.That(queue.Count, Is.EqualTo(2));
                Assert.That(item, Is.EqualTo("1"));
            }
        }

        [TestFixture]
        public class When_Enqueue_is_called
        {
            [Test]
            public void It_adds_item_to_the_queue()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});
                queue.Enqueue("4");

                Assert.That(queue.Count, Is.EqualTo(4));
                Assert.That(queue.Contains("4"));
            }
        }

        [TestFixture]
        public class When_EnqueueRange_is_called
        {
            [Test]
            public void It_adds_items_to_the_queue()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});
                queue.EnqueueRange(new List<string> {"4", "5", "6"});

                Assert.That(queue.Count, Is.EqualTo(6));
                Assert.That(queue.Contains("6"));
            }

            [Test]
            public void It_throws_exception_when_items_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new ThreadSafeQueue<string>().EnqueueRange(null));
            }
        }

        [TestFixture]
        public class When_Peek_is_called
        {
            [Test]
            public void It_returns_first_item_from_the_queue_but_does_not_remove_it()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});
                var item = queue.Peek();

                Assert.That(queue.Count, Is.EqualTo(3));
                Assert.That(item, Is.EqualTo("1"));
            }
        }

        [TestFixture]
        public class When_TrimExcess_is_called
        {
            [Test]
            public void It_does_not_throw_exception()
            {
                Assert.DoesNotThrow(() => new ThreadSafeQueue<string>().TrimExcess());
            }
        }

        [TestFixture]
        public class When_TryDequeue_is_called
        {
            [Test]
            public void It_returns_false_if_queue_is_empty()
            {
                var queue = new ThreadSafeQueue<string>();
                string output;

                Assert.That(queue.TryDequeue(out output), Is.False);
            }

            [Test]
            public void It_returns_true_if_queue_contains_at_least_one_item()
            {
                var queue = new ThreadSafeQueue<string>(new List<string> {"1", "2", "3"});
                string output;

                Assert.That(queue.TryDequeue(out output), Is.True);
                Assert.That(output, Is.EqualTo("1"));
            }
        }
    }
}