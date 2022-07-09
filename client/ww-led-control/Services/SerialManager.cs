using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ww_led_control.Services
{
    public class SerialManager
    {
        public enum Commands : byte
        {
            TURN_OFF = 0x00,
            TURN_ON = 0x01,
            CHANGE_COLOR = 0x02,
            SET_HEALTH = 0x03,
            ANIMATION_WINDWAKER_START = 0x04,
            ANIMATION_WINDWAKER_BEAT = 0x05,
            ANIMATION_OPEN = 0x06,
            ANIMATION_OPEN_CHEST = 0x07,
            ANIMATION_SWIMMING = 0x08,
        }

        private SerialPort serialPort;

        public event Action<bool> OnSerialOpen;
        public void NotifySerialChange(bool open) => OnSerialOpen?.Invoke(open);

        public bool Initialize(string portName, int baudRate)
        {
            if (IsOpen())
                return true;

            try
            {
                serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
                serialPort.DataReceived += new
                    SerialDataReceivedEventHandler(PortDataReceived);
                serialPort.Open();

                NotifySerialChange(serialPort.IsOpen);
            } catch {
                return false;
                NotifySerialChange(serialPort.IsOpen);
            }
            return true;
        }

        public void WriteMessage(byte[] messageBytes)
        {
            if (!IsOpen())
                return;

            System.Diagnostics.Debug.WriteLine("Sending Command: " + messageBytes[0]);
            serialPort.Write(messageBytes, 0, 4);
        }
        private void PortDataReceived(object sender,
            SerialDataReceivedEventArgs e)
        {
            byte[] messageBytes = { 0x00, 0x00, 0x00, 0x00 };
            int message = serialPort.Read(messageBytes, 0, 4);

            /*
            for (int i = 0; i < messageBytes.Length; i++)
            {
                Console.WriteLine(messageBytes[i]);
            }
            */
        }

        public bool GetPortState()
        {
            return serialPort.IsOpen;
        }

        public void Stop()
        {
            if (serialPort == null || !serialPort.IsOpen)
                return;

            serialPort.Close();
            serialPort.DataReceived -= new
                    SerialDataReceivedEventHandler(PortDataReceived);
            NotifySerialChange(serialPort.IsOpen);
        }

        public bool IsOpen()
        {
            if(serialPort == null)
                return false;

            return serialPort.IsOpen;
        }

        public string[] GetAvailiblePorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool SelectPort(string portName)
        {
            if (serialPort.IsOpen)
                serialPort.Close();

            serialPort.PortName = portName;
            serialPort.Open();
            return true;
        }
    }
}
