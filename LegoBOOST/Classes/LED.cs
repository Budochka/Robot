using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

[assembly: InternalsVisibleTo("LegoBOOSTNetTests")]

namespace LegoBOOSTNet.Classes
{
    class LED : ILED
    {
        private readonly IConnection _connection;
        private readonly Ports _port;
        private Color _color;

        internal LED(IConnection connection, Ports port)
        {
            _connection = connection;
            _port = port;

            LoggerHelper.Instance.Debug("Button constructor called");
        }

        internal void FireOnColorChanged()
        {
            EventHandler handler = OnColorChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public void SetColor(Color color)
        {
            _color = color;

            if (!_connection.WriteValue(CreateMessage()))
            {
                LoggerHelper.Instance.Debug("LED::SetColor - failed to set color");
                throw new Exception("Failed to set color");
            }

            LoggerHelper.Instance.Debug($"LED::SetColor color = {color} called");
        }

        public async void SetColorAsync(Color color)
        {
            _color = color;

            await _connection.WriteValueAsync(CreateMessage());

            LoggerHelper.Instance.Debug("LED::SetColorAsync called");
        }

        public event EventHandler OnColorChanged;

        private byte[] CreateMessage()
        {
            var message = new byte[8]; //8 is packed length for color setting

            message[0] = (byte)message.Length;
            message[1] = 0x00; //by default
            message[2] = 0x81; //by default for messages sent to some port
            message[3] = (byte)_port;
            message[4] = 0x11; //by default
            message[5] = 0x51; //by default
            message[6] = 0x00; //by default
            message[7] = (byte)_color; //color value

            LoggerHelper.Instance.Debug($"LED::Message {BitConverter.ToString(message)} created");

            return message;
        }
    }
}
