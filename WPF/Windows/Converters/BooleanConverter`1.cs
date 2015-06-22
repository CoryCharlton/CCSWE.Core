using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CCSWE.Windows.Converters
{
    /// <summary>
    /// A generic <see cref="IValueConverter"/> that allows you to specify true/false values
    /// </summary>
    /// <typeparam name="T">The type of the true/false values</typeparam>
    public class BooleanConverter<T> : IValueConverter
    {
        #region Constructor
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The value that represents false
        /// </summary>
        public T False { get; set; }

        /// <summary>
        /// The value that represents false
        /// </summary>
        public T True { get; set; }
        #endregion

        #region Public Methods
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
	    #endregion
    }
}
