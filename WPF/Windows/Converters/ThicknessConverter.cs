using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CCSWE.Windows.Converters
{
    public class ThicknessConverter: IValueConverter
    {
        #region Public Properties
        public ThicknessSides IgnoreThicknessSides { get; set; }
        #endregion

        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var returnValue = new Thickness(0);

            if (value is Thickness)
            {
                var originalThickness = (Thickness)value;

                if (!IgnoreThicknessSides.HasFlag(ThicknessSides.Bottom))
                {
                    returnValue.Bottom = originalThickness.Bottom;
                }

                if (!IgnoreThicknessSides.HasFlag(ThicknessSides.Left))
                {
                    returnValue.Left = originalThickness.Left;
                }

                if (!IgnoreThicknessSides.HasFlag(ThicknessSides.Right))
                {
                    returnValue.Right = originalThickness.Right;
                }

                if (!IgnoreThicknessSides.HasFlag(ThicknessSides.Top))
                {
                    returnValue.Top = originalThickness.Top;
                }
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
