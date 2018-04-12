using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CCSWE.Core.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TimeSpanExtensionsTest
    {
        [TestFixture]
        public class When_ToFriendlyString_is_called
        {
            [Test]
            public void It_should_return_formatted_string_for_value_equal_to_1_hour()
            {
                var timespan = TimeSpan.FromHours(1);

                Assert.That(timespan.ToFriendlyString(), Is.EqualTo("01:00:00"));
                Assert.That(timespan.ToFriendlyString(true), Is.EqualTo("01:00:00.000"));
            }

            public void It_should_return_formatted_string_for_value_equal_to_24_hours()
            {
                var timespan = TimeSpan.FromHours(1);

                Assert.That(timespan.ToFriendlyString(), Is.EqualTo("1.00:00:00"));
                Assert.That(timespan.ToFriendlyString(true), Is.EqualTo("1.00:00:00.000"));
            }

            [Test]
            public void It_should_return_formatted_string_for_value_greater_than_1_hour()
            {
                var timespan = new TimeSpan(0, 2, 3, 4, 5);

                Assert.That(timespan.ToFriendlyString(), Is.EqualTo("02:03:04"));
                Assert.That(timespan.ToFriendlyString(true), Is.EqualTo("02:03:04.005"));
            }

            [Test]
            public void It_should_return_formatted_string_for_value_greater_than_24_hours()
            {
                var timespan = new TimeSpan(1,2,3,4,5);

                Assert.That(timespan.ToFriendlyString(), Is.EqualTo("1.02:03:04"));
                Assert.That(timespan.ToFriendlyString(true), Is.EqualTo("1.02:03:04.005"));
            }

            [Test]
            public void It_should_return_formatted_string_for_value_less_than_1_hour()
            {
                var timespan = new TimeSpan(0, 0, 3, 4, 5);

                Assert.That(timespan.ToFriendlyString(), Is.EqualTo("03:04"));
                Assert.That(timespan.ToFriendlyString(true), Is.EqualTo("03:04.005"));
            }
        }
    }
}
