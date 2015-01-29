﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManusInterface
{
    class FingerIdSetting
    {
        public static readonly DependencyProperty fingerIdProperty = DependencyProperty.RegisterAttached("fingerId",
            typeof(string), typeof(FingerIdSetting), new FrameworkPropertyMetadata(null));

        public int GetMyProperty(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return Convert.ToInt32(element.GetValue(fingerIdProperty));
        }
        public static void SetMyProperty(UIElement element, string value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(fingerIdProperty, value);
        }
    }
}
