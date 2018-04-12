using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CCSWE.Core.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DateTimeExtensionsTest
    {
        private static DateTime _sourceDate = new DateTime(2016, 11, 1, 11, 30, 30, 500);

        [TestFixture]
        public class When_ToEndDate_is_called
        {
            [Test]
            public void It_should_return_correct_end_date()
            {
                Assert.That(_sourceDate.ToEndDate(), Is.EqualTo(new DateTime(2016, 11, 1, 23, 59, 59, 999, _sourceDate.Kind)));
            }
        }

        [TestFixture]
        public class When_ToStartDate_is_called
        {
            [Test]
            public void It_should_return_correct_end_date()
            {
                Assert.That(_sourceDate.ToStartDate(), Is.EqualTo(new DateTime(2016, 11, 1, 0, 0, 0, 0, _sourceDate.Kind)));
            }
        }
    }
}
