namespace CCSWE.Windows.Converters
{
    public class ReversedBooleanConverter: BooleanConverter<bool>
    {
        #region Constructor
        public ReversedBooleanConverter(): base(false, true) { }
	    #endregion    
    }
}
