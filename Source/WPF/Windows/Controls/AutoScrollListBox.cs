using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CCSWE.Windows.Controls
{
    public class AutoScrollListBox: ListBox
    {
        #region Constructor
        public AutoScrollListBox()
        {
            Loaded += AutoScrollListBox_Loaded;
            SelectionChanged += AutoScrollListBox_SelectionChanged;
        }
        #endregion

        #region Private Fields
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty LazyLoadCommandProperty = DependencyProperty.Register("LazyLoadCommand", typeof(ICommand), typeof(AutoScrollListBox), new PropertyMetadata(null));
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(AutoScrollListBox), new PropertyMetadata(false));
        #endregion

        #region Public Properties
        public ICommand LazyLoadCommand 
        {
            get { return (ICommand)GetValue(LazyLoadCommandProperty); }
            set { SetValue(LazyLoadCommandProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        #endregion

        #region Private Methods
        private void AutoScrollListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = this.GetDescendantByType<ScrollViewer>();
            if (scrollViewer == null)
            {
                //TODO: Should retry...
                return;
            }

            scrollViewer.ScrollChanged += AutoScrollListBox_ScrollChanged;
        }

        private void AutoScrollListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                var scrollViewer = sender as ScrollViewer;
                if (scrollViewer == null)
                {
                    return;
                }

                var ratio = scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;
                if (ratio >= 0.9)
                //if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
                {
                    if (LazyLoadCommand != null && LazyLoadCommand.CanExecute(null))
                    {
                        LazyLoadCommand.Execute(null);
                    }
                }

                //Debug.WriteLine(scrollViewer.HorizontalOffset + " / " + scrollViewer.ScrollableWidth + " -- " + scrollViewer.VerticalOffset + " / " + scrollViewer.ScrollableHeight + " (" + (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight) + ")");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AutoScrollListBox_ScrollChanged: " + ex);
                throw;
            }
        }

        private void AutoScrollListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItem == null)
            {
                return;
            }

            #region Test Code
            UpdateLayout();
            #endregion

            ScrollIntoView(SelectedItem);
        }
        
        #endregion
    }
}
