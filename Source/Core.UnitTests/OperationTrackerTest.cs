using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CCSWE.Core.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class OperationTrackerTest
    {
        [TestFixture]
        public class When_BeginOperation_is_called
        {
            [Test]
            public void It_should_increment_OperationsRunning()
            {
                var operationTracker = new OperationTracker();

                operationTracker.BeginOperation();
                Assert.That(operationTracker.OperationsRunning, Is.EqualTo(1));
            }

            [Test]
            public void It_should_invoke_setTargetPropertyAction_with_a_value_of_true()
            {
                var isOperationRunning = false;
                var operationTracker = new OperationTracker(value => { isOperationRunning = value; });

                operationTracker.BeginOperation();
                Assert.That(isOperationRunning, Is.True);
            }

            [Test]
            public void It_should_set_IsOperationRunning_to_true()
            {
                var operationTracker = new OperationTracker();

                operationTracker.BeginOperation();
                Assert.That(operationTracker.IsOperationRunning, Is.True);
            }
        }

        [TestFixture]
        public class When_EndOperation_is_called
        {
            [Test]
            public void It_should_decrement_OperationsRunning()
            {
                var operationTracker = new OperationTracker();

                operationTracker.BeginOperation();
                operationTracker.EndOperation();

                Assert.That(operationTracker.OperationsRunning, Is.EqualTo(0));
            }

            [Test]
            public void It_should_invoke_setTargetPropertyAction_with_a_value_of_false()
            {
                var isOperationRunning = false;
                var operationTracker = new OperationTracker(value => { isOperationRunning = value; });

                operationTracker.BeginOperation();
                operationTracker.EndOperation();

                Assert.That(isOperationRunning, Is.False);
            }

            [Test]
            public void It_should_not_decrement_OperationsRunning_below_zero()
            {
                var operationTracker = new OperationTracker();

                operationTracker.EndOperation();
                Assert.That(operationTracker.OperationsRunning, Is.EqualTo(0));
            }

            [Test]
            public void It_should_set_IsOperationRunning_to_true()
            {
                var operationTracker = new OperationTracker();

                operationTracker.BeginOperation();
                operationTracker.EndOperation();

                Assert.That(operationTracker.IsOperationRunning, Is.False);

            }
        }

    }
}
