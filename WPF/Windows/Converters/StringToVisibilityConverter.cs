using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CCSWE.Windows.Converters
{
    /// <summary>
    /// An <see cref="IValueConverter"/> that converts a <see cref="string"/> into a <see cref="Visibility"/>
    /// </summary>
    public class StringToVisibilityConverter: IValueConverter
    {
        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                return string.IsNullOrWhiteSpace(stringValue) ? Visibility.Collapsed : Visibility.Visible;
            }

            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
