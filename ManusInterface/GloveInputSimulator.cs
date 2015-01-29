﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManusMachina;
using System.Windows.Input;

namespace ManusInterface
{
    class GloveInputSimulator
    {
        private static GLOVE_EULER SENSITIVITY = new GLOVE_EULER(20, 10, 20);
        private static GLOVE_EULER DEADZONE_LEFT = new GLOVE_EULER(10, 10, 10);
        private static GLOVE_EULER DEADZONE_RIGHT = new GLOVE_EULER(10, 10, 10);

        private const float FINGER_THRESHOLD = 0.3F;

        private Thread simulationThread;
        private uint gloveIndex;
        private double[] mouseRemainder;

        public GloveInputSimulator(uint index)
        {
            Manus.ManusInit();

            this.gloveIndex = index;
            mouseRemainder = new double[2];

            simulationThread = new Thread(new ThreadStart(Simulate));
            simulationThread.Start();
        }

        void Simulate()
        {
            GLOVE_STATE state;
            GLOVE_EULER center, lastOffset;

            while (Manus.ManusGetState(gloveIndex, out state, true) != Manus.SUCCESS) ;
            Manus.ManusQuaternionToEuler(out center, ref state.data.Quaternion);
            lastOffset = center;

            while (true)
            {
                if (Manus.ManusGetState(gloveIndex, out state, true) != Manus.SUCCESS)
                    continue;

                GLOVE_EULER angles;
                Manus.ManusQuaternionToEuler(out angles, ref state.data.Quaternion);
                GLOVE_EULER offset = (angles - center).ToDegrees();

                if (Math.Abs(lastOffset.x - offset.x) > Math.PI)
                    lastOffset = offset;

                if (state.data.RightHand)
                    OutputRight(center, offset, lastOffset);
                else
                    OutputLeft();

                lastOffset = offset;
            }
        }

        void OutputRight(GLOVE_EULER center, GLOVE_EULER offset, GLOVE_EULER lastOffset)
        {
            double mouseX = 0.0, mouseY = 0.0;

            // Move the mouse horizontally according to the distance traveled
            if (Math.Abs(offset.x) > DEADZONE_RIGHT.x)
                mouseX -= (lastOffset.x - offset.x) * SENSITIVITY.x;

            // Use a quadratic function to increase acceleration proportional to the roll
            if (Math.Abs(offset.y) > DEADZONE_RIGHT.y)
                mouseX -= Math.Sign(offset.y) * Math.Pow(offset.y / (100.0 / SENSITIVITY.y), 2);

            // Add the remainder from the previous truncation
            mouseX += mouseRemainder[0];

            // todo nan detection for tan > 90 degrees
            // mouse_x = (int) (tan(yprOffset[1])*15); // old

            // Move the mouse vertically according to the distance traveled
            if (Math.Abs(offset.z) > DEADZONE_RIGHT.z)
                mouseY += (lastOffset.z - offset.z) * SENSITIVITY.z;

            // Add the remainder from the previous truncation
            mouseY += mouseRemainder[1];

            // Truncate the mouse and save the remainder
            int truncX = (int)Math.Truncate(mouseX);
            int truncY = (int)Math.Truncate(mouseY);
            mouseRemainder[0] = mouseX - truncX;
            mouseRemainder[1] = mouseY - truncY;

            // Move the mouse
            Mouse.move(truncX, truncY);
        }

        void OutputLeft()
        {
        }
    }
}
