using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using CCSWE.Collections.Specialized;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CCSWE.Core.UnitTests.Collections.Specialized
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class NameValueCollectionExtensionsTest
    {
        private static readonly NameValueCollection collection = new NameValueCollection { {"ValidEntry", "ValidEntry" } };

        [TestFixture]
        public class When_GetValueAs_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_key_does_not_exist()
            {
                Assert.That(collection.GetValueAs("InvalidEntry", "DefaultValue"), Is.EqualTo("DefaultValue").IgnoreCase);
            }

            [Test]
            public void It_does_not_throw_exception_when_key_exists()
            {
                Assert.That(collection.GetValueAs<string>("ValidEntry"), Is.EqualTo("ValidEntry").IgnoreCase);
            }

            [Test]
            public void It_throws_exception_when_key_is_null_or_whitespace()
            {
                Assert.Throws<ArgumentException>(() => collection.GetValueAs<string>(null));
                Assert.Throws<ArgumentException>(() => collection.GetValueAs<string>(" "));
                Assert.Throws<ArgumentException>(() => collection.GetValueAs<string>("\t"));

                Assert.Throws<ArgumentException>(() => collection.GetValueAs(null, string.Empty));
                Assert.Throws<ArgumentException>(() => collection.GetValueAs(" ", string.Empty));
                Assert.Throws<ArgumentException>(() => collection.GetValueAs("\t", string.Empty));
            }
        }

        [TestFixture]
        public class When_TryGetValueAs_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_key_does_not_exist()
            {
                string appSettingValue;

                Assert.That(collection.TryGetValueAs("InvalidAppValidAppSetting", out appSettingValue), Is.EqualTo(false));
            }

            [Test]
            public void It_does_not_throw_exception_when_key_exists()
            {
                string entryValue;

                Assert.That(collection.TryGetValueAs("ValidEntry", out entryValue), Is.EqualTo(true));
                Assert.That(entryValue, Is.EqualTo("ValidEntry").IgnoreCase);
            }

            [Test]
            public void It_throws_exception_when_key_is_null_or_whitespace()
            {
                string appSettingValue;

                Assert.Throws<ArgumentException>(() => collection.TryGetValueAs(null, out appSettingValue));
                Assert.Throws<ArgumentException>(() => collection.TryGetValueAs(" ", out appSettingValue));
                Assert.Throws<ArgumentException>(() => collection.TryGetValueAs("\t", out appSettingValue));
            }
        }
    }
}
