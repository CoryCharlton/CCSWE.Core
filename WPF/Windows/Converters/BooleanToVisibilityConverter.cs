using System.Windows;

namespace CCSWE.Windows.Converters
{
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        #region Constructor
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
	    #endregion
    }
}
