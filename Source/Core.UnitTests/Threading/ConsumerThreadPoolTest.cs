using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CCSWE.Threading;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UnusedVariable
namespace CCSWE.Core.UnitTests.Threading
{
    [TestFixture]
    public class ConsumerThreadPoolTest
    {
        [TestFixture]
        public class When_CancellationToken_is_cancelled
        {
            [Test]
            public void It_completes_adding()
            {
                var cancellationTokenSource = new CancellationTokenSource();

                Func<int, bool> processItem = i =>
                {
                    if (i > 0)
                    {
                        Thread.Sleep(10);
                    }

                    return true;
                };

                using (var threadPool = new ConsumerThreadPool<int>(2, processItem, cancellationTokenSource.Token))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    Thread.Sleep(20);

                    cancellationTokenSource.Cancel();

                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    Assert.That(threadPool.IsIndeterminate, Is.False);
                    Assert.That(threadPool.TotalItems, Is.EqualTo(100));
                }
            }

            [Test]
            public void It_stops_processing()
            {
                var cancellationTokenSource = new CancellationTokenSource();

                Func<int, bool> processItem = i =>
                {
                    if (i > 0)
                    {
                        Thread.Sleep(10);
                    }

                    return true;
                };

                using (var threadPool = new ConsumerThreadPool<int>(2, processItem, cancellationTokenSource.Token))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    Thread.Sleep(20);

                    cancellationTokenSource.Cancel();

                    Assert.That(threadPool.ItemsProcessed < threadPool.TotalItems);
                    Assert.That(threadPool.IsCancelled);
                }
            }
        }

        [TestFixture]
        public class When_Constructor_is_called
        {
            [Test]
            public void It_does_not_throw_exception()
            {
                Assert.DoesNotThrow(() => { using (var threadPool = new ConsumerThreadPool<int>(2, i => true)) {} });
                Assert.DoesNotThrow(() => { using (var threadPool = new ConsumerThreadPool<int>(2, i => true, CancellationToken.None)) { } });
            }

            [Test]
            public void It_throws_exception_when_consumerThreads_is_equal_to_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new ConsumerThreadPool<int>(0, i => true));
                Assert.Throws<ArgumentOutOfRangeException>(() => new ConsumerThreadPool<int>(0, i => true, CancellationToken.None));
            }

            [Test]
            public void It_throws_exception_when_consumerThreads_is_less_than_zero()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new ConsumerThreadPool<int>(-1, i => true));
                Assert.Throws<ArgumentOutOfRangeException>(() => new ConsumerThreadPool<int>(-1, i => true, CancellationToken.None));
            }

            [Test]
            public void It_throws_exception_when_processItem_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new ConsumerThreadPool<int>(2, null));
                Assert.Throws<ArgumentNullException>(() => new ConsumerThreadPool<int>(2, null, CancellationToken.None));
            }
        }

        [TestFixture]
        public class When_ConsumerThread_is_running
        {
            [Test]
            public async Task It_reports_failed_when_processItem_returns_true()
            {
                using (var threadPool = new ConsumerThreadPool<int>(2, item => false))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    threadPool.CompleteAdding();
                    await threadPool.WaitForCompletion();

                    Assert.That(threadPool.ItemsFailed, Is.EqualTo(threadPool.TotalItems));
                }
            }

            [Test]
            public async Task It_reports_progress()
            {
                using (var threadPool = new ConsumerThreadPool<int>(2, item => false))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    threadPool.CompleteAdding();
                    await threadPool.WaitForCompletion();

                    Assert.That(threadPool.Progress, Is.EqualTo(100));
                }
            }

            [Test]
            public async Task It_reports_successful_when_processItem_returns_true()
            {
                using (var threadPool = new ConsumerThreadPool<int>(2, item => true))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    threadPool.CompleteAdding();
                    await threadPool.WaitForCompletion();

                    Assert.That(threadPool.ItemsSuccessful, Is.EqualTo(threadPool.TotalItems));
                }
            }
        }

        [TestFixture]
        public class When_WaitForCompletion_is_called
        {
            [Test]
            [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
            public void It_does_not_throw_exception_if_ignoreIsAddingCompleted_is_true()
            {
                var cancellationTokenSource = new CancellationTokenSource();

                using (var threadPool = new ConsumerThreadPool<int>(2, item => true, cancellationTokenSource.Token))
                {
                    for (var i = 0; i < 100; i++)
                    {
                        threadPool.Add(i);
                    }

                    Task.Run(async () => { await Task.Delay(100); cancellationTokenSource.Cancel(); });

                    // ReSharper disable once AccessToDisposedClosure
                    Assert.DoesNotThrowAsync(() => threadPool.WaitForCompletion(true));
                }
            }

            [Test]
            public void It_throws_exception_if_adding_is_not_completed()
            {
                using (var threadPool = new ConsumerThreadPool<int>(2, item => { Thread.Sleep(1000); return true; }))
                {
                    for (var i = 0; i < 10; i++)
                    {
                        threadPool.Add(i);
                    }

                    // ReSharper disable once AccessToDisposedClosure
                    Assert.ThrowsAsync<InvalidOperationException>(() => threadPool.WaitForCompletion());
                }
            }
        }
    }
}
