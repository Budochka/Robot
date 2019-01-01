using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Interfaces;
using LegoBOOST.Helpers;

namespace LegoBOOST.Classes
{
    class DistanceColorSensor : IDistanceColorSensor
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;

        internal DistanceColorSensor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;

            LoggerHelper.Instance.Debug("DistanceColorSensor constructor called");
        }

        public event EventHandler<SensorEventArgs> OnChange;
    }
}
