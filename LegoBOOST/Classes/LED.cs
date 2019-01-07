﻿using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Runtime.InteropServices.WindowsRuntime;
using LegoBOOST.Constants;
using LegoBOOST.Helpers;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
{
    class LED : ILED
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;
        private Color _color;

        internal LED(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
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
            var buffer = CreateMessage(color).AsBuffer();

            AsyncHelpers.RunSync<GattCommunicationStatus>(() => _characteristic.WriteValueAsync(buffer).AsTask());

            LoggerHelper.Instance.Debug($"LED::SetColor color = {color} called");
        }

        public async void SetColorAsync(Color color)
        {
            _color = color;
            var buffer = CreateMessage(color).AsBuffer();

            await _characteristic.WriteValueAsync(buffer);

            LoggerHelper.Instance.Debug("LED::SetColorAsync called");
        }

        public event EventHandler OnColorChanged;

        private byte[] CreateMessage(Color color)
        {
            var message = new byte[8]; //8 is packed length for color setting

            message[0] = (byte)message.Length;
            message[1] = 0x00; //by default
            message[2] = 0x81; //by default for messages sent to some port
            message[3] = (byte)_port;
            message[4] = 0x11; //by default
            message[5] = 0x51; //by default
            message[6] = 0x00; //by default
            message[7] = (byte)color; //color value

            LoggerHelper.Instance.Debug($"LED::Message {BitConverter.ToString(message)} created");

            return message;
        }
    }
}
