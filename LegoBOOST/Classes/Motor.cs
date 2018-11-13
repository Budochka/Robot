using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using LegoBOOST.Constants;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
{
    class Motor : IMotor
    {
        private GattCharacteristic _characteristic;
        private Ports _port;

        Motor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;
        }

        public int Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async void SetSpeedTimed(uint seconds, int speed)
        {
            byte[] bytes = new byte[255];

            IBuffer buffer = bytes.AsBuffer();

            await _characteristic.WriteValueAsync(buffer);
        }
    }
}
