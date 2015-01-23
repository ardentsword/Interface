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
        [DllImport("User32.dll", EntryPoint = "SendInput", SetLastError = true)]
        private static extern int SendKeyboardInput(int nInputs, KEYBDINPUT[] pInputs, int cbSize);

#pragma warning disable 0649 // Disable 'field never assigned' warning
        struct KEYBDINPUT
        {
            public const INPUT_TYPE type = INPUT_TYPE.KEYBOARD;
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
            input[0].wVk = (ushort)KeyInterop.VirtualKeyFromKey(k);
            SendKeyboardInput(1, input, Marshal.SizeOf(input));
        }

        public static void release(Key k)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].wVk = (ushort)KeyInterop.VirtualKeyFromKey(k);
            input[0].dwFlags = KEYEVENTF_KEYUP;
            SendKeyboardInput(1, input, Marshal.SizeOf(input));
        }

        public static void write(Key c)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[2];
            input[0].wVk = (ushort)KeyInterop.VirtualKeyFromKey(c);
            input[1].wVk = (ushort)KeyInterop.VirtualKeyFromKey(c);
            input[1].dwFlags = KEYEVENTF_KEYUP;
            SendKeyboardInput(2, input, Marshal.SizeOf(input));
        }
    }
}
