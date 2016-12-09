using System.Collections.Specialized;

namespace CCSWE.Collections.Specialized
{
    /// <summary>
    /// Contains extension methods for <see cref="NameValueCollection"/>.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified key from the <see cref='NameValueCollection'/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="collection">The <see cref="NameValueCollection"/> to retrieve the value from.</param>
        /// <param name="key">The key associated with the value being retrieved.</param>
        /// <returns>The value associated with the specified key.</returns>
        public static T GetValueAs<T>(this NameValueCollection collection, string key)
        {
            Ensure.IsNotNull(nameof(collection), collection);
            Ensure.IsNotNullOrWhitespace(nameof(key), key);

            var stringValue = collection[key];

            return Converter.ConvertValue<T>(stringValue);
        }

        /// <summary>
        /// Gets the value associated with the specified key from the <see cref='NameValueCollection'/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="collection">The <see cref="NameValueCollection"/> to retrieve the value from.</param>
        /// <param name="key">The key associated with the value being retrieved.</param>
        /// <param name="defaultValue">The default value to be returned if the specified key does not exist or the cast fails.</param>
        /// <returns>The value associated with the specified key.</returns>
        public static T GetValueAs<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            Ensure.IsNotNull(nameof(collection), collection);
            Ensure.IsNotNullOrWhitespace(nameof(key), key);

            var stringValue = collection[key];

            return string.IsNullOrWhiteSpace(stringValue) ? defaultValue : Converter.SafeConvert(stringValue, defaultValue);
        }

        /// <summary>
        /// Trys to get the value associated with the specified key from the <see cref='NameValueCollection'/>.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="collection">The <see cref="NameValueCollection"/> to retrieve the value from.</param>
        /// <param name="key">The key associated with the value being retrieved.</param>
        /// <param name="value">The value being retrieved.</param>
        /// <returns>True if the key exists. False is not.</returns>
        public static bool TryGetValueAs<T>(this NameValueCollection collection, string key, out T value)
        {
            Ensure.IsNotNull(nameof(collection), collection);
            Ensure.IsNotNullOrWhitespace(nameof(key), key);

            var stringValue = collection[key];

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                value = default(T);
                return false;
            }

            value = Converter.SafeConvert(stringValue, default(T));

            return true;
        }
    }
}
