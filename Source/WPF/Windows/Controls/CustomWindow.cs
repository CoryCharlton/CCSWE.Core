using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CCSWE.Windows.Controls
{
    public class CustomWindow: Window
    {
        #region Constructor
        static CustomWindow()
        {
            BackgroundProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(Brushes.White));
            BorderBrushProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(103,163,219))));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }

        public CustomWindow()
        {
            //TODO: Add CommandBinding for SystemCommands.ShowSystemMenuCommand
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ActiveBorderBrushProperty = DependencyProperty.Register("ActiveBorderBrush", typeof (Brush), typeof (CustomWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 125, 167))));

        public static readonly DependencyProperty CaptionButtonForegroundProperty = DependencyProperty.Register("CaptionButtonForeground", typeof (Brush), typeof (CustomWindow), new FrameworkPropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CaptionButtonHoverBackgroundProperty = DependencyProperty.Register("CaptionButtonHoverBackground", typeof (Brush), typeof (CustomWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(54, 101, 179))));

        public static readonly DependencyProperty CaptionButtonHoverBorderBrushProperty = DependencyProperty.Register("CaptionButtonHoverBorderBrush", typeof(Brush), typeof(CustomWindow), new FrameworkPropertyMetadata(default(SolidColorBrush)));

        public static readonly DependencyProperty CaptionButtonHoverForegroundProperty = DependencyProperty.Register("CaptionButtonHoverForeground", typeof (Brush), typeof (CustomWindow), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty CaptionButtonPressedBackgroundProperty = DependencyProperty.Register("CaptionButtonPressedBackground", typeof(Brush), typeof(CustomWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(61, 96, 153))));

        public static readonly DependencyProperty CaptionButtonPressedBorderBrushProperty = DependencyProperty.Register("CaptionButtonPressedBorderBrush", typeof(Brush), typeof(CustomWindow), new FrameworkPropertyMetadata(default(SolidColorBrush)));

        public static readonly DependencyProperty CaptionButtonPressedForegroundProperty = DependencyProperty.Register("CaptionButtonPressedForeground", typeof(Brush), typeof(CustomWindow), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty ContentBorderBrushProperty = DependencyProperty.Register("ContentBorderBrush", typeof (Brush), typeof (CustomWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(88,139,186))));

        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof (Brush), typeof (CustomWindow), new FrameworkPropertyMetadata(default(SolidColorBrush)));

        public static readonly DependencyProperty ResizeGripForegroundProperty = DependencyProperty.Register("ResizeGripForeground", typeof(Brush), typeof(CustomWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 125, 167))));
        #endregion

        #region Public Properties
        public Brush ActiveBorderBrush
        {
            get { return (Brush) GetValue(ActiveBorderBrushProperty); }
            set { SetValue(ActiveBorderBrushProperty, value); }
        }

        public Brush CaptionButtonForeground
        {
            get { return (Brush)GetValue(CaptionButtonForegroundProperty); }
            set { SetValue(CaptionButtonForegroundProperty, value); }
        }

        public Brush CaptionButtonHoverBackground
        {
            get { return (Brush)GetValue(CaptionButtonHoverBackgroundProperty); }
            set { SetValue(CaptionButtonHoverBackgroundProperty, value); }
        }

        public Brush CaptionButtonHoverBorderBrush
        {
            get { return (Brush)GetValue(CaptionButtonHoverBorderBrushProperty); }
            set { SetValue(CaptionButtonHoverBorderBrushProperty, value); }
        }

        public Brush CaptionButtonHoverForeground
        {
            get { return (Brush)GetValue(CaptionButtonHoverForegroundProperty); }
            set { SetValue(CaptionButtonHoverForegroundProperty, value); }
        }

        public Brush CaptionButtonPressedBackground
        {
            get { return (Brush)GetValue(CaptionButtonPressedBackgroundProperty); }
            set { SetValue(CaptionButtonPressedBackgroundProperty, value); }
        }

        public Brush CaptionButtonPressedBorderBrush
        {
            get { return (Brush)GetValue(CaptionButtonPressedBorderBrushProperty); }
            set { SetValue(CaptionButtonPressedBorderBrushProperty, value); }
        }

        public Brush CaptionButtonPressedForeground
        {
            get { return (Brush)GetValue(CaptionButtonPressedForegroundProperty); }
            set { SetValue(CaptionButtonPressedForegroundProperty, value); }
        }

        public Brush ContentBorderBrush
        {
            get { return (Brush)GetValue(ContentBorderBrushProperty); }
            set { SetValue(ContentBorderBrushProperty, value); }
        }

        public Brush ResizeGripForeground
        {
            get { return (Brush)GetValue(ResizeGripForegroundProperty); }
            set { SetValue(ResizeGripForegroundProperty, value); }
        }

        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }
        #endregion

        #region Private Methods
        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
	    #endregion
    }
}
