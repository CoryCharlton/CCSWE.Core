using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CCSWE.Windows.Converters
{
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
        public T False { get; set; }
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
