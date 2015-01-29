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
            GLOVE_EULER center, lastOffset;
            double[] remainder = new double[2];

            Manus.ManusInit();

            while (Manus.ManusGetState(0, out state, true) != Manus.SUCCESS);
            Manus.ManusQuaternionToEuler(out center, ref state.data.Quaternion);
            lastOffset = center;

            while (true)
            {
                if (Manus.ManusGetState(0, out state, true) != Manus.SUCCESS)
                    continue;

                GLOVE_EULER angles;
                Manus.ManusQuaternionToEuler(out angles, ref state.data.Quaternion);
                GLOVE_EULER offset = (angles - center).ToDegrees();

                // Move the mouse horizontally according to the distance traveled
                double mouseX = -(lastOffset.x - offset.x) * 20.0;

                // Use a quadratic function to increase acceleration proportional to the roll
                mouseX -= Math.Sign(offset.y) * Math.Pow(offset.y / 10, 2);

                // Add the remainder from the previous truncation
                mouseX += remainder[0];

                // todo nan detection for tan > 90 degrees
                // mouse_x = (int) (tan(yprOffset[1])*15); // old

                // Move the mouse vertically according to the distance traveled
                double mouseY = (lastOffset.z - offset.z) * 20.0;

                // Add the remainder from the previous truncation
                mouseY += remainder[1];

                // Truncate the mouse and save the remainder
                int truncX = (int)Math.Truncate(mouseX);
                int truncY = (int)Math.Truncate(mouseY);
                remainder[0] = mouseX - truncX;
                remainder[1] = mouseY - truncY;

                // Move the mouse
                Mouse.move(truncX, truncY);

                lastOffset = offset;
            }

            Manus.ManusExit();
        }
    }
}
