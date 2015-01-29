﻿using System;
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

namespace ManusInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManusGUI : Window
    {
        private TextBox selectedKeybindBox;
        private bool mouseListenActive;
        private GloveInputSimulator simulator;

        Key[][] keyBindings = new Key[2][];

        public ManusGUI()
        {
            simulator = new GloveInputSimulator();
            InitializeComponent();    
            Key[] keyBindingsLeftHand = new Key[5];
            Key[] keyBindingsRightHand = new Key[5];
            keyBindings[0] = keyBindingsLeftHand;
            keyBindings[1] = keyBindingsRightHand;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider1Value.Text = slider1.Value.ToString();
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider2Value.Text = slider2.Value.ToString();
        }

        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider3Value.Text = slider3.Value.ToString();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            Debug.Write("Closing called");
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
    }
}
