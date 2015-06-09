using System.Windows;
using PropertyChanged;

namespace CCSWE.Windows.Controls
{
    [ImplementPropertyChanged]
    public partial class BusySpinner
    {
        #region Constructor
        public BusySpinner()
        {
            InitializeComponent();
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SpinnerHeightProperty = DependencyProperty.Register("SpinnerHeight", typeof(double), typeof(BusySpinner), new PropertyMetadata(75.0));
        public static readonly DependencyProperty SpinnerWidthProperty = DependencyProperty.Register("SpinnerWidth", typeof(double), typeof(BusySpinner), new PropertyMetadata(75.0));
        #endregion

        #region Public Properties
        public double SpinnerHeight
        {
            get { return (double)GetValue(SpinnerHeightProperty); }
            set { SetValue(SpinnerHeightProperty, value); }
        }

        public double SpinnerWidth
        {
            get { return (double)GetValue(SpinnerWidthProperty); }
            set { SetValue(SpinnerWidthProperty, value); }
        }
        #endregion
    }
}
