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
using System.Threading;
using System.Threading.Tasks;
using ManusMachina;
using System.Windows.Input;

namespace ManusInterface
{
    class GloveInputSimulator
    {
        private static GLOVE_VECTOR SENSITIVITY = new GLOVE_VECTOR(10, 20, 20);
        private static GLOVE_VECTOR DEADZONE_LEFT = new GLOVE_VECTOR(20, 20, 20);
        private static GLOVE_VECTOR DEADZONE_RIGHT = new GLOVE_VECTOR(20, 20, 20);

        private const float FINGER_THRESHOLD = 0.5F;

        private Thread simulationThread;
        private uint gloveIndex;
        private double[] mouseRemainder;

        private bool running;

        public Key[][] fingerKeyBindings;
        public MouseButton[][] fingerMouseBindings;

        public GloveInputSimulator(uint index)
        {
            this.gloveIndex = index;
            mouseRemainder = new double[2];
            running = true;
            fingerKeyBindings = new Key[2][];
            fingerKeyBindings[0] = new Key[5];
            fingerKeyBindings[1] = new Key[5];

            fingerMouseBindings = new MouseButton[2][];
            fingerMouseBindings[0] = new MouseButton[5];
            fingerMouseBindings[1] = new MouseButton[5];

            simulationThread = new Thread(new ThreadStart(Simulate));
            simulationThread.SetApartmentState(ApartmentState.STA);
            simulationThread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        void Simulate()
        {
            GLOVE_STATE state;
            GLOVE_VECTOR center, gravity, lastOffset;

            while (Manus.ManusGetState(gloveIndex, out state, true) != Manus.SUCCESS)
            {
                if (!running)
                    return;
                Thread.Sleep(100);
            }

            Manus.ManusGetGravity(out gravity, ref state.data.Quaternion);
            Manus.ManusGetEuler(out center, ref state.data.Quaternion, ref gravity);
            lastOffset = center;

            while (running)
            {
                if (Manus.ManusGetState(gloveIndex, out state, true) != Manus.SUCCESS)
                    continue;

                GLOVE_VECTOR angles;
                Manus.ManusGetGravity(out gravity, ref state.data.Quaternion);
                Manus.ManusGetEuler(out angles, ref state.data.Quaternion, ref gravity);

                int fingersClamped = 0;
                for (int i = 0; i < state.data.Fingers.Length; i++)
                    if (state.data.Fingers[i] > FINGER_THRESHOLD && state.data.Fingers[i] > 0.0)
                        fingersClamped++;

                if (fingersClamped >= state.data.Fingers.Length)
                    center = angles;

                GLOVE_VECTOR offset = (angles - center).ToDegrees();

                if (Math.Abs(lastOffset.x - offset.x) > Math.PI)
                    lastOffset = offset;

                if (state.data.RightHand)
                    OutputRight(offset, lastOffset);
                else
                    OutputLeft(offset, fingersClamped);

                int hand = state.data.RightHand ? 1 : 0;
                for (int i = 0; i < 5; i++)
                {
                    if (fingerKeyBindings[hand][i] == Key.System)
                    {
                        if (state.data.Fingers[i] > FINGER_THRESHOLD)
                            Mouse.press(fingerMouseBindings[hand][i]);
                        else
                            Mouse.release(fingerMouseBindings[hand][i]);
                    }
                    else if (fingerKeyBindings[hand][i] != Key.None)
                    {
                        if (state.data.Fingers[i] > FINGER_THRESHOLD)
                            Keyboard.press(fingerKeyBindings[hand][i]);
                        else
                            Keyboard.release(fingerKeyBindings[hand][i]);
                    }
                }

                lastOffset = offset;
            }
        }

        void OutputRight(GLOVE_VECTOR offset, GLOVE_VECTOR lastOffset)
        {
            double mouseX = 0.0, mouseY = 0.0;

            // Move the mouse horizontally according to the distance traveled
            mouseX -= (lastOffset.z - offset.z) * SENSITIVITY.z;

            // Use a quadratic function to increase acceleration proportional to the roll
            if (Math.Abs(offset.x) > DEADZONE_RIGHT.x)
                mouseX += Math.Sign(offset.x) * Math.Pow(offset.x / (100.0 / SENSITIVITY.x), 2);

            // Add the remainder from the previous truncation
            mouseX += mouseRemainder[0];

            // todo nan detection for tan > 90 degrees
            // mouse_x = (int) (tan(yprOffset[1])*15); // old

            // Move the mouse vertically according to the distance traveled
            mouseY += (lastOffset.y - offset.y) * SENSITIVITY.y;

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

        void OutputLeft(GLOVE_VECTOR offset, int fingersClamped)
        {
            if (fingersClamped >= 3)
            {
                Keyboard.press(Key.W);
                Keyboard.release(Key.S);

                if (offset.z < -DEADZONE_LEFT.z)
                {
                    Keyboard.release(Key.D);
                    Keyboard.press(Key.A);
                }
                else if (offset.z > DEADZONE_LEFT.z)
                {
                    Keyboard.press(Key.D);
                    Keyboard.release(Key.A);
                }
                else
                {
                    Keyboard.release(Key.A);
                    Keyboard.release(Key.D);
                }
            }
            else
            {
                Keyboard.release(Key.W);
                Keyboard.release(Key.S);
                Keyboard.release(Key.A);
                Keyboard.release(Key.D);
            }
        }
    }
}
