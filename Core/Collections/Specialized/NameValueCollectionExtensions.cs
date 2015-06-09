using System.Collections.Specialized;

namespace CCSWE.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        public static T GetValueAs<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            var stringValue = collection[key];
            if (string.IsNullOrWhiteSpace(stringValue))
                return defaultValue;

            return Converter.ConvertValue<T>(stringValue);
        }

        public static T GetValueAs<T>(this NameValueCollection collection, string key)
        {
            var stringValue = collection[key];
            return Converter.ConvertValue<T>(stringValue);
        }

        public static bool TryGetValueAs<T>(this NameValueCollection collection, string key, out T value)
        {
            var stringValue = collection[key];
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                value = default(T);
                return false;
            }

            value = Converter.ConvertValue<T>(stringValue);
            return true;
        }

    }
}
