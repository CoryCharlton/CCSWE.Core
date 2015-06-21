using System.Windows;
using System.Windows.Media;

namespace CCSWE
{
    public static class VisualExtensions
    {
        public static FrameworkElement GetDescendantByName(this Visual element, string name)
        {
            if (element == null)
            {
                return null;
            }

            //TODO: Should check the name here...

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = frameworkElement.GetDescendantByName(name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }

            return null;
        }

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
