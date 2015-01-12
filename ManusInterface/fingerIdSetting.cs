using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManusInterface
{
    class fingerIdSetting
    {
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.RegisterAttached("MyProperty",
            typeof(string), typeof(fingerIdSetting), new FrameworkPropertyMetadata(null));

        public static string GetMyProperty(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (string)element.GetValue(MyPropertyProperty);
        }
        public static void SetMyProperty(UIElement element, string value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(MyPropertyProperty, value);
        }
    }
}
