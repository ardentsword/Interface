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
                return new int[] {  1, 2  };
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
