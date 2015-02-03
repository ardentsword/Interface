/**
 * Copyright (C) 2015 Manus Machina
 *
 * This file is part of the Manus SDK.
 * 
 * Manus SDK is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Manus SDK is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Manus SDK. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
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
