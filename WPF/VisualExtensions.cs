using System.Windows;
using System.Windows.Media;

namespace CCSWE
{
    public static class VisualExtensions
    {
        public static T GetDescendantByType<T>(this Visual element) where T : class
        {
            if (element == null)
            {
                return default(T);
            }

            if (element.GetType() == typeof(T))
            {
                return element as T;
            }

            T foundElement = null;

            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = visual.GetDescendantByType<T>();
                if (foundElement != null)
                {
                    break;
                }
            }

            return foundElement;
        }
    }
}
