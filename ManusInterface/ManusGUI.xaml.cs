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

        public ManusGUI()
        {
            Manus.ManusInit();
            simulators = new GloveInputSimulator[2];
            simulators[0] = new GloveInputSimulator(0);
            simulators[1] = new GloveInputSimulator(1);
            InitializeComponent();
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

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            List<String> keyBindings = new List<String>();

            StringBuilder builder = new StringBuilder();
            builder.Append("k");
            keyBindings.Add("0");
            keyBindings.Add(converKeyToHex(lThumb.Text));

            keyBindings.Add(builder.ToString());      
        }

        private string converKeyToHex(string sendKey)
        {
            return "";
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
            if (selectedKeybindBox !=null)
            selectedKeybindBox.Text=e.Key.ToString();
            mouseListenActive = false;
        }

        private void OnMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (selectedKeybindBox != null && mouseListenActive)
                selectedKeybindBox.Text = e.ChangedButton.ToString();

            mouseListenActive = true;
        }
    }
}
