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
        public GLOVE_EULER mouseSensitivity = new GLOVE_EULER(20,10,20);
        public GLOVE_EULER leftGloveDeadzones = new GLOVE_EULER(20, 20, 20);
        public GLOVE_EULER rightGloveDeadzones = new GLOVE_EULER(10, 10, 10);

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

                keyBindings[gloveIndex][fingerIndex] = e.Key;
                mouseListenActive = false;
            }
        }

        private void OnMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (selectedKeybindBox != null && mouseListenActive)
                selectedKeybindBox.Text = e.ChangedButton.ToString();

            mouseListenActive = true;
        }
        
        private void mouseSensitivity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           mouseSensitivityYawTb.Text= mouseSensitivityYaw.Value.ToString();
           mouseSensitivityPitchTb.Text = mouseSensitivityPitch.Value.ToString();
           mouseSensitivityRollTb.Text = mouseSensitivityRoll.Value.ToString();
           mouseSensitivity = new GLOVE_EULER((float)mouseSensitivityYaw.Value, (float)mouseSensitivityPitch.Value, (float)mouseSensitivityRoll.Value);
        }


        private void deadZoneLeft_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            deadZoneLeftYawTb.Text = deadZoneLeftYaw.Value.ToString();
            deadZoneLeftPitchTb.Text = deadZoneLeftPitch.Value.ToString();
            deadZoneLeftRollTb.Text = deadZoneLeftRoll.Value.ToString();
            leftGloveDeadzones = new GLOVE_EULER((float)deadZoneLeftYaw.Value, (float)deadZoneLeftPitch.Value, (float)deadZoneLeftRoll.Value);
        }

        private void deadZoneRight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            deadZoneRightYawTb.Text = deadZoneRightYaw.Value.ToString();
            deadZoneRightPitchTb.Text = deadZoneRightPitch.Value.ToString();
            deadZoneRightRollTb.Text = deadZoneRightRoll.Value.ToString();
            rightGloveDeadzones = new GLOVE_EULER((float)deadZoneRightYaw.Value, (float)deadZoneRightPitch.Value, (float)deadZoneRightRoll.Value);
        }
    }
}
