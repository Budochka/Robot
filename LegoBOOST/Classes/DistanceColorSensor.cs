using System;
using System.Runtime.InteropServices.WindowsRuntime;
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

            SetNotifications();

            LoggerHelper.Instance.Debug("DistanceColorSensor constructor called");
        }

        private void SetNotifications()
        {
            var buffer = ConnectionConstants.CMD_SUBSCRIBE_DISTANCE_COLOR;
            buffer[3] = (byte) _port;

            LoggerHelper.Instance.Debug($"DistanceColorSensor::SetNotifications {BitConverter.ToString(buffer)} created");

            AsyncHelpers.RunSync<GattCommunicationStatus>(() => _characteristic.WriteValueAsync(buffer.AsBuffer()).AsTask());
        }

        public event EventHandler<SensorEventArgs> OnChange;
    }
}
