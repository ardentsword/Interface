using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ManusInterface
{
    class Mouse
    {
        [DllImport("User32.dll")]
        private static extern uint SendInput(uint nInputs, MOUSEINPUT[] pInputs, int cbSize);

        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        const uint MOUSEEVENTF_HWHEEL = 0x1000;
        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
        const uint MOUSEEVENTF_WHEEL = 0x0800;
        const uint MOUSEEVENTF_XDOWN = 0x0080;
        const uint MOUSEEVENTF_XUP = 0x0100;

        const int XBUTTON1 = 1;
        const int XBUTTON2 = 2;

#pragma warning disable 0649 // Disable 'field never assigned' warning
        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public INPUT_TYPE type;
            public Int32 dx;
            public Int32 dy;
            public UInt32 mouseData;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr extraInfo;
        }
#pragma warning restore 0649

        private static uint ButtonToFlag(MouseButton b, MouseButtonState state)
        {
            switch (b)
            {
                case MouseButton.Left:
                    return (state == MouseButtonState.Pressed) ?
                        MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP;
                case MouseButton.Middle:
                    return (state == MouseButtonState.Pressed) ?
                        MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP;
                case MouseButton.Right:
                    return (state == MouseButtonState.Pressed) ?
                        MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP;
                case MouseButton.XButton1:
                case MouseButton.XButton2:
                    return (state == MouseButtonState.Pressed) ?
                        MOUSEEVENTF_XDOWN : MOUSEEVENTF_XUP;
            }
            return 0;
        }
        private static uint ButtonToData(MouseButton b)
        {
            switch (b)
            {
                case MouseButton.XButton1:
                    return XBUTTON1;
                case MouseButton.XButton2:
                    return XBUTTON2;
            }
            return 0;
        }

        public static void click(MouseButton b)
        {
            MOUSEINPUT[] input = new MOUSEINPUT[2];
            input[0].type = INPUT_TYPE.MOUSE;
            input[0].mouseData = ButtonToData(b);
            input[0].flags = ButtonToFlag(b, MouseButtonState.Pressed);
            input[1].type = INPUT_TYPE.MOUSE;
            input[1].mouseData = ButtonToData(b);
            input[1].flags = ButtonToFlag(b, MouseButtonState.Released);
            SendInput(2, input, 2 * Marshal.SizeOf(typeof(MOUSEINPUT)));
        }

        public static void move(int x, int y, int wheel = 0)
        {
            MOUSEINPUT[] input = new MOUSEINPUT[1];
            input[0].type = INPUT_TYPE.MOUSE;
            input[0].dx = x;
            input[0].dy = y;
            input[0].flags = MOUSEEVENTF_MOVE | MOUSEEVENTF_WHEEL;
            input[0].mouseData = (UInt32)wheel;
            SendInput(1, input, Marshal.SizeOf(typeof(MOUSEINPUT)));
        }

        public static void press(MouseButton b)
        {
            MOUSEINPUT[] input = new MOUSEINPUT[1];
            input[0].type = INPUT_TYPE.MOUSE;
            input[0].mouseData = ButtonToData(b);
            input[0].flags = ButtonToFlag(b, MouseButtonState.Pressed);
            SendInput(1, input, Marshal.SizeOf(typeof(MOUSEINPUT)));
        }

        public static void release(MouseButton b)
        {
            MOUSEINPUT[] input = new MOUSEINPUT[1];
            input[0].type = INPUT_TYPE.MOUSE;
            input[0].mouseData = ButtonToData(b);
            input[0].flags = ButtonToFlag(b, MouseButtonState.Released);
            SendInput(1, input, Marshal.SizeOf(typeof(MOUSEINPUT)));
        }

        public static bool isPressed(MouseButton b)
        {
            switch (b)
            {
                case MouseButton.Left:
                    return System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed;
                case MouseButton.Middle:
                    return System.Windows.Input.Mouse.MiddleButton == MouseButtonState.Pressed;
                case MouseButton.Right:
                    return System.Windows.Input.Mouse.RightButton == MouseButtonState.Pressed;
                case MouseButton.XButton1:
                    return System.Windows.Input.Mouse.XButton1 == MouseButtonState.Pressed;
                case MouseButton.XButton2:
                    return System.Windows.Input.Mouse.XButton2 == MouseButtonState.Pressed;
            }
            return false;
        }
    }
}
