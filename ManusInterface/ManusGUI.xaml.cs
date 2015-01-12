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

namespace ManusInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManusGUI : Window
    {
        private String selectedComName="";
        private String selectedDroneComName = "";
        private TextBox selectedKeybindBox;
        private bool mouseListenActive;
        
        //TODO: set to false after debugging
        private bool droneMode = true;

        public ManusGUI()
        {
            InitializeComponent();
            comNames.ItemsSource = SerialPort.GetPortNames();
            comNamesDrone.ItemsSource = SerialPort.GetPortNames();            
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

        private void connectDevice(object sender, RoutedEventArgs e)
        {
            CreateDeviceConnection.SetComPort(selectedComName);           
            //first check if the Docking station is correctly connected
            /*
            if (CreateDeviceConnection.SetComPort())
            {
                portNumberTxt.Text = CreateDeviceConnection.getPortName();
                //then check if the left and right glove are responding
                if (CreateDeviceConnection.checkDevices())
                {
                    leftGloveDetected.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);
                    rightGloveDetected.Fill = new SolidColorBrush(System.Windows.Media.Colors.Green);
                }
            }
            else
            {
                Debug.WriteLine("No connected COM device detected please check connection");
                
            }
             * */
        }

        private void connectDroneDevice(object sender, RoutedEventArgs e)
        {
            CreateDeviceConnection.SetDroneComPort(selectedDroneComName);
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            List<String> keyBindings = new List<String>();

            StringBuilder builder = new StringBuilder();
            builder.Append("k");
            keyBindings.Add("0");
            keyBindings.Add(converKeyToHex(lThumb.Text));

            keyBindings.Add(builder.ToString());

            foreach (String keyBind in keyBindings)
            {
                CreateDeviceConnection.sendMessage(keyBind, CreateDeviceConnection.selectedGlovePort);
            }            
        }

        
        private string converKeyToHex(string sendKey)
        {
            return "";
        }

        private void comNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedComName = comNames.SelectedValue.ToString();
        }
        private void comNamesDrone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDroneComName = comNamesDrone.SelectedValue.ToString();
        }


        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            Debug.Write("Closing called");
            CreateDeviceConnection.terminateConnection();
        }

        private void EnableDroneMode(object sender, RoutedEventArgs e)
        {
            droneMode = true;
            CreateDeviceConnection.SetDroneComPort(selectedDroneComName);
            CreateDeviceConnection.SetComPort(selectedComName);  
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
