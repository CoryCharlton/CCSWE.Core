using System.Configuration;
using CCSWE.Collections.Specialized;

namespace CCSWE.Configuration
{
    /// <summary>
    /// Helper class for retrieving values from <seealso cref="ConfigurationManager.AppSettings"/> 
    /// </summary>
    public static class AppSettings
    {
        #region Public Methods
        /// <summary>
        /// Gets the value associated with the specified key from <see cref='ConfigurationManager.AppSettings'/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key associated with the value being retrieved.</param>
        /// <returns>The value associated with the specified key.</returns>
        public static T GetValueAs<T>(string key)
        {
            Ensure.IsNotNullOrWhitespace(nameof(key), key);

            return ConfigurationManager.AppSettings.GetValueAs<T>(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key from <see cref='ConfigurationManager.AppSettings'/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key associated with the value being retrieved.</param>
        /// <param name="defaultValue">The default value to be returned if the specified key does not exist or the cast fails.</param>
        /// <returns>The value associated with the specified key.</returns>
        public static T GetValueAs<T>(string key, T defaultValue)
        {
            Ensure.IsNotNullOrWhitespace(nameof(key), key);

            return ConfigurationManager.AppSettings.GetValueAs(key, defaultValue);
        }

        /// <summary>
        /// Trys to get the value associated with the specified key from the <see cref='ConfigurationManager.AppSettings'/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key associated with the value being retrieved.</param>
        /// <param name="value">The value being retrieved.</param>
        /// <returns>True if the key exists. False is not.</returns>
        public static bool TryGetValueAs<T>(string key, out T value)
        {
            Ensure.IsNotNullOrWhitespace(nameof(key), key);

            return ConfigurationManager.AppSettings.TryGetValueAs(key, out value);
        }
        #endregion
    }
}
