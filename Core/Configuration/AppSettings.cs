using System.Configuration;
using CCSWE.Collections.Specialized;

namespace CCSWE.Configuration
{
    /// <summary>
    /// Helper class for reading values from <seealso cref="ConfigurationManager.AppSettings"/> 
    /// </summary>
    public static class AppSettings
    {
        #region Public Methods
        public static T GetValueAs<T>(string key)
        {
            return ConfigurationManager.AppSettings.GetValueAs<T>(key);
        }

        public static T GetValueAs<T>(string key, T defaultValue)
        {
            return ConfigurationManager.AppSettings.GetValueAs(key, defaultValue);
        }

        public static bool TryGetValueAs<T>(string key, out T value)
        {
            return ConfigurationManager.AppSettings.TryGetValueAs(key, out value);
        }
        #endregion
    }
}
