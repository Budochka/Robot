using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Interfaces;
using NLog;

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

            LogManager.GetCurrentClassLogger().Debug("Button constructor called");
        }

        public event EventHandler OnButtonPressed;
        public event EventHandler OnButtonReleased;
    }
}