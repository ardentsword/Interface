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
        [DllImport("User32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

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

        const uint MAPVK_VK_TO_CHAR = 2;
        const uint MAPVK_VK_TO_VSC = 0;
        const uint MAPVK_VSC_TO_VK = 1;
        const uint MAPVK_VSC_TO_VK_EX = 3;

        private static uint ScanCodeFromKey(Key k)
        {
            return MapVirtualKey((uint)KeyInterop.VirtualKeyFromKey(k), MAPVK_VK_TO_VSC);
        }

        public static void press(Key k)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].wScan = (ushort)ScanCodeFromKey(k);
            input[0].dwFlags = KEYEVENTF_SCANCODE;
            SendInput(1, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }

        public static void release(Key k)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].wScan = (ushort)ScanCodeFromKey(k);
            input[0].dwFlags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
            SendInput(1, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }

        public static void write(Key c)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[2];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].wScan = (ushort)ScanCodeFromKey(c);
            input[0].dwFlags = KEYEVENTF_SCANCODE;
            input[1].type = INPUT_TYPE.KEYBOARD;
            input[1].wScan = (ushort)ScanCodeFromKey(c);
            input[1].dwFlags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
            SendInput(2, input, 2 * Marshal.SizeOf(typeof(KEYBDINPUT)));
        }
    }
}
