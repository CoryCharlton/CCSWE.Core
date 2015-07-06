using System.Windows;
using System.Windows.Controls;

namespace CCSWE.Windows.Controls
{
    /// <summary>
    /// An extension of the <see cref="Border"/> control that is designed for use in style templates
    /// </summary>
    public class TemplateBorder: Border
    {
        static TemplateBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TemplateBorder), new FrameworkPropertyMetadata(typeof(TemplateBorder)));
        }
    }
}
