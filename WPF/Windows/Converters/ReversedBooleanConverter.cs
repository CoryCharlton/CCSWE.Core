using System.Windows.Data;

namespace CCSWE.Windows.Converters
{
    /// <summary>
    /// An <see cref="IValueConverter"/> that converts a <see cref="bool"/> to the opposite value
    /// </summary>
    public class ReversedBooleanConverter: BooleanConverter<bool>
    {
        #region Constructor
        public ReversedBooleanConverter(): base(false, true) { }
	    #endregion    
    }
}
