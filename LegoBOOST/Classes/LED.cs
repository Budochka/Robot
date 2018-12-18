using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
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
        }

        public bool SetColor(Color color)
        {
            _color = color;
            return true;
        }

        public event EventHandler OnColorChange;
    }
}
