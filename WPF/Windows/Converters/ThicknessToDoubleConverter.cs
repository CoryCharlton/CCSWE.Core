using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CCSWE.Windows.Converters
{
    public class ThicknessToDoubleConverter: IValueConverter
    {
        #region Public Properties
        public ThicknessSides Side { get; set; }
        #endregion

        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness)
            {
                var thickness = (Thickness) value;

                switch (Side)
                {
                    case ThicknessSides.Bottom:
                    {
                        return thickness.Bottom;
                    }
                    case ThicknessSides.Left:
                    {
                        return thickness.Left;
                    }
                    case ThicknessSides.Right:
                    {
                        return thickness.Right;
                    }
                    case ThicknessSides.Top:
                    {
                        return thickness.Top;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
	    #endregion
    }
}
