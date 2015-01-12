using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManusInterface
{
    class hexDictonary
    {
        Dictionary<String, byte> hexDict = new Dictionary<string, byte>();
        private hexDictonary(){
            hexDict.Add("KEY_LEFT_CTRL",0x80);
            hexDict.Add("KEY_LEFT_SHIFT",0x81);
            hexDict.Add("KEY_LEFT_ALT",0x82);
            hexDict.Add("KEY_LEFT_GUI",0x83);
            hexDict.Add("KEY_RIGHT_CTRL",0x84);
            hexDict.Add("KEY_RIGHT_SHIFT",0x85);
            hexDict.Add("KEY_RIGHT_ALT",0x86);
            hexDict.Add("KEY_RIGHT_GUI",0x87);
            hexDict.Add("KEY_UP_ARROW",0xDA);
            hexDict.Add("KEY_DOWN_ARROW",0xD9);
            hexDict.Add("KEY_LEFT_ARROW",0xD8);
            hexDict.Add("KEY_RIGHT_ARROW",0xD7);
            hexDict.Add("KEY_BACKSPACE",0xB2);
            hexDict.Add("KEY_TAB",0xB3);
            hexDict.Add("KEY_RETURN",0xB0);
                        hexDict.Add("KEY_ESC",0xB1);
                        hexDict.Add("KEY_INSERT",0xD1);
                        hexDict.Add("KEY_DELETE",0xD4);
                        hexDict.Add("KEY_PAGE_UP",0xD3);
                        hexDict.Add("KEY_PAGE_DOWN",0xD6);
                        hexDict.Add("KEY_HOME",0xD2);
                        hexDict.Add("KEY_END",0xD5);
                        hexDict.Add("KEY_CAPS_LOCK",0xC1);
                        hexDict.Add("KEY_F1",0xC2);
                        hexDict.Add("KEY_F2",0xC3);
                        hexDict.Add("KEY_F3",0xC4);
            hexDict.Add("KEY_F4",0xC5);
            hexDict.Add("KEY_F5",0xC6);
            hexDict.Add("KEY_F6",0xC7);
            hexDict.Add("KEY_F7",0xC8);
            hexDict.Add("KEY_F8",0xC9);
            hexDict.Add("KEY_F9",0xCA);
            hexDict.Add("KEY_F10",0xCB);
            hexDict.Add("KEY_F11",0xCC);
            hexDict.Add("KEY_F12",0xCD);
        }

        public byte getHexFromDictonary(String keyBind)
        {
            if (hexDict.ContainsKey(keyBind))
                return hexDict[keyBind];
            else if (keyBind.Length==1)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(keyBind);
                // Convert the decimal value to a hexadecimal value in string form.
               String hexFromVal=String.Format("{0:X}", value);
              byte[]bytesFromHex= System.Text.Encoding.Unicode.GetBytes(hexFromVal);
              return bytesFromHex[0];
            }
            else
            {
                throw new KeyBindNotFoundException(keyBind);
            }
        }
	
    }
}
