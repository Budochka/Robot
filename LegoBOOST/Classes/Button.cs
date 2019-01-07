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

        internal void FireEvent(byte data)
        {
            switch (data)
            {
                case 0x00:
                {
                    EventHandler handler = OnButtonPressed;
                    handler?.Invoke(this, EventArgs.Empty);
                    break;
                }

                case 0x01:
                {
                    EventHandler handler = OnButtonReleased;
                    handler?.Invoke(this, EventArgs.Empty);
                    break;
                }
            }
        }
    }
}