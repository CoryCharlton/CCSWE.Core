using System.Windows;
using System.Windows.Media;

namespace CCSWE
{
    public static class VisualExtensions
    {
        /// <summary>
        /// Searches the visual tree for a <see cref="FrameworkElement"/> with the supplied name
        /// </summary>
        /// <param name="element">The parent <see cref="Visual"/> to begin searching from</param>
        /// <param name="name">The name of the target <see cref="FrameworkElement"/></param>
        /// <returns>The <see cref="FrameworkElement"/> if found</returns>
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

        /// <summary>
        /// Searches the visual tree for the first child of the supplied type
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Visual"/> we are searching for</typeparam>
        /// <param name="element">The parent <see cref="Visual"/> to begin searching from</param>
        /// <returns>The descendant if found</returns>
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
