using System.ComponentModel;
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
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BusySpinner), new PropertyMetadata(null));
        #endregion

        #region Public Properties
        [DefaultValue(75.0)]
        public double SpinnerHeight
        {
            get { return (double)GetValue(SpinnerHeightProperty); }
            set { SetValue(SpinnerHeightProperty, value); }
        }

        [DefaultValue(75.0)]
        public double SpinnerWidth
        {
            get { return (double)GetValue(SpinnerWidthProperty); }
            set { SetValue(SpinnerWidthProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion
    }
}
