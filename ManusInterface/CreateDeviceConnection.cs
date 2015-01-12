using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace ManusInterface
{
    /**
     * Handles the initalization of the Docking station COM port and handles all the communication between Dock and interface
     * **/
    public static class CreateDeviceConnection
    {
        public static SerialPort selectedGlovePort;
        public static SerialPort selectedDronePort;
        static Thread readThread = new Thread(Read);
        static Thread sendThread = new Thread(Send);

        private static String returnMessage = "";
        private static bool readPort = true;
        private static bool sendPort = true;
        private static int connectionTimeOut = 1500;
        private static int bautRate = 115200;

        //holds the converted YPR values received from the glove
        private static int[] convertedValues = new int[3];

        /**
         * Loops through all connected USB devices and pinging them voor the message of DOCK
         * The responding COM port will be set as connected USB device
         **/
        public static bool SetComPort()
        {
            //TODO: Works for first try for connection, if second attempt is made it breaks because already running
            /*
            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                selectedPort = new SerialPort(port, 115200);
                //selectedPort.ReadTimeout = connectionTimeOut;
                //selectedPort.WriteTimeout = connectionTimeOut;
                selectedPort.Open();
                readThread.Start();

                //if the response is correct docking station is on the current port, else close connection and try next
                if (checkResponse(StaticKeys.DETECT_DOCK))
                    return true;
                 selectedPort.Close();
                 readThread.Join();
            }
            readThread.Join();
            return false;*/
            return false;
        }

                public static bool SetComPortDrone()
        {
            return false;
        }

        /**
         * Checks if the left and right glove are alive and kicking 
         **/
        public static bool checkDevices()
        {
            if (!checkResponse(StaticKeys.DETECT_LEFT))
            {
                Debug.WriteLine("Left glove not detected please check connection");
                //leftGloveDetected.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                return false;
            }
            if (!checkResponse(StaticKeys.DETECT_RIGHT))
            {
                Debug.WriteLine("Right glove not detected please check connection");
                //rightGloveDetected.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);
                return false;
            }
            return true;
        }

        /**
         * Validates if a send message gets the correct response
         * @message: The message to test for appropriate response
        **/
        private static bool checkResponse(String message)
        {
            selectedGlovePort.Write(message);
            Thread.Sleep(connectionTimeOut);
            if (returnMessage.Equals(StaticKeys.AOK))
                return true;
            return false;
        }

        /**
         * Reads all incoming messages from a seperate thread
         **/
       static string message;

        private static void Read()
        {
            while (readPort)
            {    
                try
                {
                    handleMessage();
                   // Debug.WriteLine(selectedPort.ReadLine());
                    //message = selectedPort.ReadLine();
                    message=selectedGlovePort.ReadLine();
                    message=message.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                    char[] dataArray = message.ToCharArray();
                    
                    //size check for correctly recieved message
                    if (dataArray.Length==6)
                        convertReceivedArray(dataArray);
                }
                 catch (ArgumentException e)
                {
                    Debug.WriteLine(e);
                }
                catch (InvalidOperationException e)
                {
                    Debug.WriteLine(e);
                    returnMessage = StaticKeys.ERR;
                    readPort = false;
                }
                catch (TimeoutException e) {
                    Debug.WriteLine("Connection TimedOut");
                    Debug.WriteLine(e);
                    returnMessage = StaticKeys.ERR;
                    readPort = false;
                }
                catch (System.IO.IOException e)
                {
                    Debug.WriteLine(e);
                    returnMessage = StaticKeys.ERR;
                    readPort = false;
                }
            }
        }

        private static void handleMessage()
        {
            //if(inDroneMode)
        }

        //TODO: make a differentation between send to drone and send to glove
        private static void Send()
        {
            while (sendPort)
            {
                try
                {
                    //TODO: discuss with Wesley about throttle
                    int throttle = SendToQcopter.remapValue(100, 40, 220);
                    int yaw = SendToQcopter.remapValue(convertedValues[0],180,540);
                    int pitch = SendToQcopter.remapValue(convertedValues[1],40,220);
                    int roll = SendToQcopter.remapValue(convertedValues[2],40,220);
                    byte[]sendCommand=SendToQcopter.buildSendCommand(throttle, yaw, pitch, roll);
                    selectedDronePort.Write(sendCommand, 0, sendCommand.Length); //write them to serial
                }
                catch (ArgumentException e)
                {
                    Debug.WriteLine(e);
                }
                catch (InvalidOperationException e)
                {
                    Debug.WriteLine(e);
                    returnMessage = StaticKeys.ERR;
                    readPort = false;
                }
                catch (TimeoutException e)
                {
                    Debug.WriteLine("Connection TimedOut");
                    Debug.WriteLine(e);
                    returnMessage = StaticKeys.ERR;
                    readPort = false;
                }
                catch (System.IO.IOException e)
                {
                    Debug.WriteLine(e);
                    returnMessage = StaticKeys.ERR;
                    readPort = false;
                }
            }
        }

        private static void convertReceivedArray(char[] dataArray)
        {
            //yaw is comprised of 2 seperate chars (low and high byte)
          int yaw1 =Convert.ToInt32(dataArray[2]);
          int yaw2 =Convert.ToInt32(dataArray[3]);
           convertedValues[0] = int.Parse(yaw1.ToString() + yaw2.ToString());
           convertedValues[1] = Convert.ToInt32(dataArray[4]);
           convertedValues[2] = Convert.ToInt32(dataArray[5]);
           Debug.WriteLine("yaw=" + convertedValues[0]+"pitch=" + convertedValues[1]+"roll=" + convertedValues[2]);
        }
        
        /**
         * Returnes the selected Ports name
         **/
        public static String getPortName()
        {
            return selectedGlovePort.PortName;
        }

        internal static void SetComPort(string selectedComName)
        {
            selectedGlovePort = new SerialPort(selectedComName, bautRate, Parity.None, 8, StopBits.One);
            selectedGlovePort.DtrEnable = true;
           //selectedPort.ReadTimeout = connectionTimeOut;
            //selectedPort.WriteTimeout = connectionTimeOut;
            selectedGlovePort.Open();
            readThread.Start();
        }

        internal static void SetDroneComPort(string selectedComName)
        {
            selectedDronePort = new SerialPort(selectedComName, bautRate, Parity.None, 8, StopBits.One);
            //selectedDronePort.DtrEnable = true;
            //selectedPort.ReadTimeout = connectionTimeOut;
            //selectedPort.WriteTimeout = connectionTimeOut;
            selectedDronePort.Open();
        }

        public static void terminateConnection()
        {
            if (selectedGlovePort != null && selectedGlovePort.IsOpen)
            {
                readPort = false;
                readThread.Join();
                selectedGlovePort.Close();
            }

            if (selectedDronePort != null && selectedDronePort.IsOpen)
            {
                sendPort = false;
                sendThread.Join();
                selectedDronePort.Close();
            }
        }

        internal static void sendMessage(string message,SerialPort selectedPort)
        {
            sendThread.Start();



        }
    }
}