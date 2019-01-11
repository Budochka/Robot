using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

namespace LegoBOOSTNet.Classes
{
    class DistanceColorSensor : IDistanceColorSensor
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;

        internal void FireEvent(Color color, int distance)
        {
            DistanceColorSensorEventArgs args = new DistanceColorSensorEventArgs() {ColorDetected = color, Distance = distance};
            var handler = OnChange;
            handler?.Invoke(this, args);
            LoggerHelper.Instance.Debug("DistanceColorSensor::FireEvent OnChange");
        }

        internal DistanceColorSensor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;

            SetNotifications();

            LoggerHelper.Instance.Debug("DistanceColorSensor constructor called");
        }

        private void SetNotifications()
        {
            //load default method format
            var buffer = ConnectionConstants.CMD_SUBSCRIBE_DISTANCE_COLOR;
            //set the port
            buffer[3] = (byte) _port;

            LoggerHelper.Instance.Debug($"DistanceColorSensor::SetNotifications {BitConverter.ToString(buffer)} created");

            AsyncHelpers.RunSync<GattCommunicationStatus>(() => _characteristic.WriteValueAsync(buffer.AsBuffer()).AsTask());
        }

        public event EventHandler<DistanceColorSensorEventArgs> OnChange;
    }
}
