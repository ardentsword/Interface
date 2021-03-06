﻿/**
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
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using ManusMachina;

namespace ManusInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManusGUI : Window
    {
        private TextBox selectedKeybindBox;
        private bool mouseListenActive;
        private GloveInputSimulator[] simulators;

        Key[][] keyBindings = new Key[2][];
        public GLOVE_VECTOR mouseSensitivity = new GLOVE_VECTOR(20,10,20);
        public GLOVE_VECTOR leftGloveDeadzones = new GLOVE_VECTOR(20, 20, 20);
        public GLOVE_VECTOR rightGloveDeadzones = new GLOVE_VECTOR(10, 10, 10);

        public ManusGUI()
        {
            Manus.ManusInit();
            simulators = new GloveInputSimulator[2];
            simulators[0] = new GloveInputSimulator(0);
            simulators[1] = new GloveInputSimulator(1);
            InitializeComponent();
            Key[] keyBindingsLeftHand = new Key[5];
            Key[] keyBindingsRightHand = new Key[5];
            keyBindings[0] = keyBindingsLeftHand;
            keyBindings[1] = keyBindingsRightHand;
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            // Cleanup
            simulators[0].Stop();
            simulators[1].Stop();
            Manus.ManusExit();
        }

        void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //How so access my currently selected tab???
        }

        private void profileInputSelected(object sender, RoutedEventArgs e)
        {
            //reset the current to white
            if (selectedKeybindBox != null)
            selectedKeybindBox.Background = Brushes.White;

            TextBox previousSelectedKeybindBox=selectedKeybindBox;

            selectedKeybindBox = (TextBox)sender;
            selectedKeybindBox.Background = Brushes.LightGray;

            //if user switched input field
            if (previousSelectedKeybindBox != null &&! previousSelectedKeybindBox.Equals(selectedKeybindBox))
                mouseListenActive = false;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (selectedKeybindBox != null)
            {
                selectedKeybindBox.Text = e.Key.ToString();
                TextBox selectedInput= (TextBox)sender;
                //TODO: once the
                //int gloveIndex = FingerIdSetting.GetMyProperty(selectedInput);
                //int fingerIndex = (int)selectedInput.GetValue(fingerIndexProperty);

                int[] temp = TemporaryLookupTable.getHandAndFingerID(selectedInput.Name);
                int gloveIndex = temp[0];
                int fingerIndex = temp[1];

                foreach (GloveInputSimulator sim in simulators)
                    sim.fingerKeyBindings[gloveIndex][fingerIndex] = e.Key;

                mouseListenActive = false;
            }
        }

        private void OnMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (selectedKeybindBox != null && mouseListenActive)
            {
                selectedKeybindBox.Text = e.ChangedButton.ToString();
                TextBox selectedInput = (TextBox)sender;
                //TODO: once the
                //int gloveIndex = FingerIdSetting.GetMyProperty(selectedInput);
                //int fingerIndex = (int)selectedInput.GetValue(fingerIndexProperty);

                int[] temp = TemporaryLookupTable.getHandAndFingerID(selectedInput.Name);
                int gloveIndex = temp[0];
                int fingerIndex = temp[1];

                foreach (GloveInputSimulator sim in simulators)
                {
                    sim.fingerKeyBindings[gloveIndex][fingerIndex] = Key.System;
                    sim.fingerMouseBindings[gloveIndex][fingerIndex] = e.ChangedButton;
                }
            }

            mouseListenActive = true;
        }
        
        private void mouseSensitivity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           mouseSensitivityYawTb.Text= mouseSensitivityYaw.Value.ToString();
           mouseSensitivityPitchTb.Text = mouseSensitivityPitch.Value.ToString();
           mouseSensitivityRollTb.Text = mouseSensitivityRoll.Value.ToString();
           mouseSensitivity = new GLOVE_VECTOR((float)mouseSensitivityYaw.Value, (float)mouseSensitivityPitch.Value, (float)mouseSensitivityRoll.Value);
        }


        private void deadZoneLeft_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            deadZoneLeftYawTb.Text = deadZoneLeftYaw.Value.ToString();
            deadZoneLeftPitchTb.Text = deadZoneLeftPitch.Value.ToString();
            deadZoneLeftRollTb.Text = deadZoneLeftRoll.Value.ToString();
            leftGloveDeadzones = new GLOVE_VECTOR((float)deadZoneLeftYaw.Value, (float)deadZoneLeftPitch.Value, (float)deadZoneLeftRoll.Value);
        }

        private void deadZoneRight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            deadZoneRightYawTb.Text = deadZoneRightYaw.Value.ToString();
            deadZoneRightPitchTb.Text = deadZoneRightPitch.Value.ToString();
            deadZoneRightRollTb.Text = deadZoneRightRoll.Value.ToString();
            rightGloveDeadzones = new GLOVE_VECTOR((float)deadZoneRightYaw.Value, (float)deadZoneRightPitch.Value, (float)deadZoneRightRoll.Value);
        }
    }
}
