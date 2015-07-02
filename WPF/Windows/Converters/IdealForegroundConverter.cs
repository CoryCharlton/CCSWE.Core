using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CCSWE.Windows.Converters
{
    public class IdealForegroundConverter : IValueConverter, IMultiValueConverter
    {
        private static IdealForegroundConverter _instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static IdealForegroundConverter()
        {
        }

        private IdealForegroundConverter()
        {
        }

        public static IdealForegroundConverter Instance
        {
            get { return _instance ?? (_instance = new IdealForegroundConverter()); }
        }


        #region Private Methods
        // From: http://www.codeproject.com/Articles/16565/Determining-Ideal-Text-Color-Based-on-Specified-Ba
        private static Color IdealForegroundColor(Color bg)
        {
            const int nThreshold = 105;
            var bgDelta = System.Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) + (bg.B * 0.114));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }
	    #endregion

        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;

            if (brush == null)
            {
                return Brushes.White;
            }

            var foreGroundBrush = new SolidColorBrush(IdealForegroundColor(brush.Color));
            foreGroundBrush.Freeze();

            return foreGroundBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var backgroundBrush = values.Length > 0 ? values[0] as Brush : null;
            var foregroundBrush = values.Length > 1 ? values[1] as Brush : null;
            
            return foregroundBrush ?? Convert(backgroundBrush, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
	    #endregion
    }
}
