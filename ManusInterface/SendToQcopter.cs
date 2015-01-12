using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManusInterface
{
    class SendToQcopter
    {
        private static byte[] takamina = new byte[11];
        private static SerialPort selectedPort;

        //TODO: always send this as first command on connection?
        public SendToQcopter(){
            //1500 default value
            takamina[1] = 220; 
            takamina[2] = 5;

            takamina[3] = 220;
            takamina[4] = 5;

            takamina[5] = 220;
            takamina[6] = 5;

            takamina[7] = 220;
            takamina[8] = 5;

            takamina[9] = 255;
        }


        //Linear transformation  Y = (X-A)/(B-A) * (D-C) + C
        public static int remapValue(float value, float from1, float to1)
        {
            float from2=2000;
            float to2=1000;
            return (int)Math.Round((value - from1) / (to1 - from1) * (from2 - to2) + to2);
        }
   

        public static byte[] buildSendCommand(int throtle, int roll, int pitch, int yaw)
        {
            takamina[0] = 101; //start bit
            takamina[10] = 102; // end bit
            // convert value's
            ushort number1 = Convert.ToUInt16(throtle);
            byte upper1 = Convert.ToByte(number1 >> 8);
            byte lower1 = Convert.ToByte(number1 & 0x00FF);
            ushort number2 = Convert.ToUInt16(roll);
            byte upper2 = Convert.ToByte(number2 >> 8);
            byte lower2 = Convert.ToByte(number2 & 0x00FF);
            ushort number3 = Convert.ToUInt16(pitch);
            byte upper3 = Convert.ToByte(number3 >> 8);
            byte lower3 = Convert.ToByte(number3 & 0x00FF);
            ushort number4 = Convert.ToUInt16(yaw);
            byte upper4 = Convert.ToByte(number4 >> 8);
            byte lower4 = Convert.ToByte(number4 & 0x00FF);

            //put them in array.
            takamina[1] = lower1;
            takamina[2] = upper1;

            takamina[3] = lower2;
            takamina[4] = upper2;

            takamina[5] = lower3;
            takamina[6] = upper3;

            takamina[7] = lower4;
            takamina[8] = upper4;

            takamina[9] = 255;

            Debug.WriteLine("send command");
            return takamina;
        }
    }
}
