using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ManusMachina
{
#pragma warning disable 0649 // Disable 'field never assigned' warning
    struct GLOVE_QUATERNION
    {
        public float w, x, y, z;

        public GLOVE_QUATERNION(float w, float x, float y, float z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static GLOVE_QUATERNION operator +(GLOVE_QUATERNION a, GLOVE_QUATERNION b)
        {
            return new GLOVE_QUATERNION(a.w + b.w, a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static GLOVE_QUATERNION operator -(GLOVE_QUATERNION a, GLOVE_QUATERNION b)
        {
            return new GLOVE_QUATERNION(a.w - b.w, a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static GLOVE_QUATERNION operator *(GLOVE_QUATERNION a, GLOVE_QUATERNION b)
        {
            return new GLOVE_QUATERNION(a.w * b.w, a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static GLOVE_QUATERNION operator /(GLOVE_QUATERNION a, GLOVE_QUATERNION b)
        {
            return new GLOVE_QUATERNION(a.w / b.w, a.x / b.x, a.y / b.y, a.z / b.z);
        }
    }

    struct GLOVE_EULER
    {
        public float x, y, z;

        public GLOVE_EULER(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static GLOVE_EULER operator +(GLOVE_EULER a, GLOVE_EULER b)
        {
            return new GLOVE_EULER(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static GLOVE_EULER operator -(GLOVE_EULER a, GLOVE_EULER b)
        {
            return new GLOVE_EULER(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static GLOVE_EULER operator *(GLOVE_EULER a, GLOVE_EULER b)
        {
            return new GLOVE_EULER(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static GLOVE_EULER operator /(GLOVE_EULER a, GLOVE_EULER b)
        {
            return new GLOVE_EULER(a.x / b.x, a.y / b.y, a.z / b.z);
        }
    }

    struct GLOVE_DATA
    {
        public bool RightHand;
        public GLOVE_QUATERNION Quaternion;
        public GLOVE_EULER Angles;

        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5)]
        public float[] Fingers;
    }

    struct GLOVE_STATE
    {
        public uint PacketNumber;
        public GLOVE_DATA data;
    }
#pragma warning restore 0649

    class Manus
    {
        public const int ERROR = -1;
        public const int SUCCESS = 0;
        public const int INVALID_ARGUMENT = 1;
        public const int OUT_OF_RANGE = 2;
        public const int DISCONNECTED = 3;

        /*! \brief Initialize the Manus SDK.
        *
        *  Must be called before any other function
        *  in the SDK.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusInit();

        /*! \brief Shutdown the Manus SDK.
        *
        *  Must be called when the SDK is no longer
        *  needed.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusExit();

        /*! \brief Get the number of gloves.
        *
        *  Get the maximum index that can be queried
        *  for the glove state.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusGetGloveCount();

        /*! \brief Get the state of a glove.
        *
        *  \param glove The glove index.
        *  \param state Output variable to receive the state.
        *  \param blocking Wait until the glove returns a value.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusGetState(uint glove, out GLOVE_STATE state, bool blocking = false);

        /*! \brief Convert a Quaternion to Euler angles.
        *
        *  Returns the Quaternion as Yaw, Pitch and Roll angles.
        *
        *  \param euler Output variable to receive the Euler angles.
        *  \param quaternion The quaternion to convert.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusQuaternionToEuler(out GLOVE_EULER euler, ref GLOVE_QUATERNION quaternion);

        /*! \brief Enable gamepad emulation.
        *
        *  Allows the SDK to convert glove data to gamepad
        *  input.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusEnableGamepad(bool enabled);

        /*! \brief Enable mouse emulation.
        *
        *  Allows the SDK to convert glove data to mouse
        *  input.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusEnableKeyboard(bool enabled);

        /*! \brief Enable keyboard emulation.
        *
        *  Allows the SDK to convert glove data to keyboard
        *  input.
        */
        [DllImport("Manus.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ManusEnableMouse(bool enabled);
    }
}
