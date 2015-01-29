using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManusInterface
{
    class HandIdSetting
    {
        public static readonly DependencyProperty handIdProperty = DependencyProperty.RegisterAttached("handId",
            typeof(string), typeof(FingerIdSetting), new FrameworkPropertyMetadata(null));

        public static int GetMyProperty(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return Convert.ToInt32(element.GetValue(handIdProperty));
        }
        public static void SetMyProperty(UIElement element, string value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(handIdProperty, value);
        }
    }
}
