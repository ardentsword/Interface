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
            public UInt16 vk;
            public UInt16 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr extrainfo;
            public UInt32 padding1;
            public UInt32 padding2;
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
            if (System.Windows.Input.Keyboard.IsKeyDown(k))
                return;

            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].scanCode = (ushort)ScanCodeFromKey(k);
            input[0].flags = KEYEVENTF_SCANCODE;
            SendInput(1, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }

        public static void release(Key k)
        {
            if (System.Windows.Input.Keyboard.IsKeyUp(k))
                return;

            KEYBDINPUT[] input = new KEYBDINPUT[1];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].scanCode = (ushort)ScanCodeFromKey(k);
            input[0].flags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
            SendInput(1, input, Marshal.SizeOf(typeof(KEYBDINPUT)));
        }

        public static void write(Key c)
        {
            KEYBDINPUT[] input = new KEYBDINPUT[2];
            input[0].type = INPUT_TYPE.KEYBOARD;
            input[0].scanCode = (ushort)ScanCodeFromKey(c);
            input[0].flags = KEYEVENTF_SCANCODE;
            input[1].type = INPUT_TYPE.KEYBOARD;
            input[1].scanCode = (ushort)ScanCodeFromKey(c);
            input[1].flags = KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE;
            SendInput(2, input, 2 * Marshal.SizeOf(typeof(KEYBDINPUT)));
        }
    }
}
