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

namespace ManusInterface
{
    //TODO: replace with fingerId and handId custom xaml properties
    static class TemporaryLookupTable
    {
        public static int[] getHandAndFingerID(String textBox)
        {
            if (textBox.Equals("lThumb"))
            {
                return new int[] { 0, 0 };
            }
            else if (textBox.Equals("lIndex"))
            {
                return new int[] { 0, 1  };
            }
            else if (textBox.Equals("lMiddle"))
            {
                return new int[] {  0, 2  };
            }
            else if (textBox.Equals("lRing"))
            {
                return new int[] { 0, 3  };
            }
            else if (textBox.Equals("lPinky"))
            {
                return new int[] { 0, 4  };
            }
            else if (textBox.Equals("rThumb"))
            {
                return new int[] { 1, 0 };
            }
            else if (textBox.Equals("rIndex"))
            {
                return new int[] {  1, 1  };
            }
            else if (textBox.Equals("rMiddle"))
            {
                return new int[] {  1, 2  };
            }
            else if (textBox.Equals("rRing"))
            {
                return new int[] {  1, 3 };
            }
            else if (textBox.Equals("rPinky"))
            {
                return new int[] {  1, 4  };
            }
            return null;
        }
    }
}
