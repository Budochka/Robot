using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Helpers;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
{
    public class Button : IButton
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;

        internal Button(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;

            LoggerHelper.Instance.Debug("Button constructor called");
        }

        public event EventHandler OnButtonPressed;
        public event EventHandler OnButtonReleased;
    }
}