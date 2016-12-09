using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CCSWE.Core.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class EnsureTest
    {
        [TestFixture]
        public class When_IsInRange_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_in_range()
            {
                Assert.DoesNotThrow(() => Ensure.IsInRange("It_does_not_throw_exception_when_in_range", 1 > 0));
            }

            [Test]
            public void It_throws_exception_when_not_in_range()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => Ensure.IsInRange("It_throws_exception_when_not_in_range", 0 > 1));
            }
        }

        [TestFixture]
        public class When_IsNotNull_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_input_is_not_null()
            {
                Assert.DoesNotThrow(() => Ensure.IsNotNull("It_does_not_throw_exception_when_input_is_not_null", new object()));
            }

            [Test]
            public void It_throws_exception_when_input_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => Ensure.IsNotNull("It_throws_exception_when_input_is_null", (object)null));
            }
        }

        [TestFixture]
        public class When_IsNotNullOrWhitespace_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_input_is_not_null()
            {
                Assert.DoesNotThrow(() => Ensure.IsNotNullOrWhitespace("It_does_not_throw_exception_when_input_is_not_null", "I am a string!"));
            }

            [Test]
            public void It_throws_exception_when_input_is_null_or_whitespace()
            {
                Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace("It_throws_exception_when_input_is_null_or_whitespace", null));
                Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace("It_throws_exception_when_input_is_null_or_whitespace", " "));
                Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrWhitespace("It_throws_exception_when_input_is_null_or_whitespace", "\t"));
            }
        }

        [TestFixture]
        public class When_IsValid_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_input_is_true()
            {
                Assert.DoesNotThrow(() => Ensure.IsValid("It_does_not_throw_exception_when_input_is_not_null", true));
            }

            [Test]
            public void It_throws_exception_when_input_is_false()
            {
                Assert.Throws<Exception>(() => Ensure.IsValid<Exception>("It_throws_exception_when_input_is_false", false));
            }
        }
    }
}
