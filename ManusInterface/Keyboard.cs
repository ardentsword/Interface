using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ManusInterface
{
    class Keyboard
    {
        [DllImport("User32.dll")]
        private static extern uint SendInput(uint nInputs, KEYBDINPUT[] pInputs, int cbSize);

#pragma warning disable 0649 // Disable 'field never assigned' warning
        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public INPUT_TYPE type;
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public uint dwExtraInfo;
        }
#pragma warning restore 0649

        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_SCANCODE = 0x0008;
        const uint KEYEVENTF_UNICODE = 0x0004;

        public static void press(Key k)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].wVk = (ushort)KeyInterop.VirtualKeyFromKey(k);
            SendInput(1, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }

        public static void release(Key k)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].wVk = (ushort)KeyInterop.VirtualKeyFromKey(k);
            input[0].dwFlags = KEYEVENTF_KEYUP;
            SendInput(1, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }

        public static void write(Key c)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[2];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].wVk = (ushort)KeyInterop.VirtualKeyFromKey(c);
            input[1].type = INPUT_TYPE.KEYBOARD;
            input[1].wVk = (ushort)KeyInterop.VirtualKeyFromKey(c);
            input[1].dwFlags = KEYEVENTF_KEYUP;
            SendInput(2, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }
    }
}
