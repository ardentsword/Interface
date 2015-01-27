using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManusMachina;

namespace ManusInterface
{
    class GloveInputSimulator
    {
        private Thread simulationThread;

        public GloveInputSimulator()
        {
            simulationThread = new Thread(new ThreadStart(Simulate));
            simulationThread.Start();
        }

        void Simulate()
        {
            GLOVE_STATE state;
            GLOVE_EULER center;
            double[] mouseError = new double[2];

            Manus.ManusInit();

            while (Manus.ManusGetState(0, out state, true) != Manus.SUCCESS);
            Manus.ManusQuaternionToEuler(out center, ref state.data.Quaternion);

            while (true)
            {
                if (Manus.ManusGetState(0, out state, true) != Manus.SUCCESS)
                    continue;

                GLOVE_EULER angles;
                Manus.ManusQuaternionToEuler(out angles, ref state.data.Quaternion);
                GLOVE_EULER offset = (angles - center).ToDegrees();

                double mouseXfloat = -Math.Sign(offset.y) * Math.Pow(offset.y / 10, 2) + mouseError[0];
                int mouseXint = (int)mouseXfloat;
                mouseError[0] = mouseXfloat - mouseXint;
                // todo nan detection for tan > 90 degrees
                // mouse_x = (int) (tan(yprOffset[1])*15); // old

                double mouseYfloat = Math.Sign(offset.z) * Math.Pow(offset.z / 10, 2) + mouseError[1];
                int mouseYint = (int)mouseYfloat;
                mouseError[1] = mouseYfloat - mouseYint;
                //mouse_y = -(int) (tan(yprOffset[2])*15); // old

                if (mouseXint != 0 || mouseYint != 0)
                {
                    Mouse.move(mouseXint, (1 - 2) * mouseYint);
                }
            }

            Manus.ManusExit();
        }
    }
}
