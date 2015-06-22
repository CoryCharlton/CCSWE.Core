using System.Windows;

namespace CCSWE.Windows.Converters
{
    /// <summary>
    /// Convert between <see cref="bool"/> and <see cref="Visibility"/>
    /// </summary>
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        #region Constructor
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
	    #endregion
    }
}
