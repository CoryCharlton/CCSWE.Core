using System;
using System.Diagnostics.CodeAnalysis;
using CCSWE.Configuration;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace CCSWE.Core.UnitTests.Configuration
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class AppSettingsTest
    {
        [TestFixture]
        public class When_GetValueAs_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_key_does_not_exist()
            {
                Assert.That(AppSettings.GetValueAs("InvalidAppValidAppSetting", "DefaultValue"), Is.EqualTo("DefaultValue").IgnoreCase);
            }

            [Test]
            public void It_does_not_throw_exception_when_key_exists()
            {
                Assert.That(AppSettings.GetValueAs<string>("ValidAppSetting"), Is.EqualTo("ValidAppSetting").IgnoreCase);
            }

            [Test]
            public void It_throws_exception_when_key_is_null_or_whitespace()
            {
                Assert.Throws<ArgumentException>(() => AppSettings.GetValueAs<string>(null));
                Assert.Throws<ArgumentException>(() => AppSettings.GetValueAs<string>(" "));
                Assert.Throws<ArgumentException>(() => AppSettings.GetValueAs<string>("\t"));

                Assert.Throws<ArgumentException>(() => AppSettings.GetValueAs(null, string.Empty));
                Assert.Throws<ArgumentException>(() => AppSettings.GetValueAs(" ", string.Empty));
                Assert.Throws<ArgumentException>(() => AppSettings.GetValueAs("\t", string.Empty));
            }
        }

        [TestFixture]
        public class When_TryGetValueAs_is_called
        {
            [Test]
            public void It_does_not_throw_exception_when_key_does_not_exist()
            {
                string appSettingValue;

                Assert.That(AppSettings.TryGetValueAs("InvalidAppValidAppSetting", out appSettingValue), Is.EqualTo(false));
            }

            [Test]
            public void It_does_not_throw_exception_when_key_exists()
            {
                string appSettingValue;

                Assert.That(AppSettings.TryGetValueAs("ValidAppSetting", out appSettingValue), Is.EqualTo(true));
                Assert.That(appSettingValue, Is.EqualTo("ValidAppSetting").IgnoreCase);
            }

            [Test]
            public void It_throws_exception_when_key_is_null_or_whitespace()
            {
                string appSettingValue;

                Assert.Throws<ArgumentException>(() => AppSettings.TryGetValueAs(null, out appSettingValue));
                Assert.Throws<ArgumentException>(() => AppSettings.TryGetValueAs(" ", out appSettingValue));
                Assert.Throws<ArgumentException>(() => AppSettings.TryGetValueAs("\t", out appSettingValue));
            }
        }
    }
}
