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

            var result = AsyncHelpers.RunSync(() => _characteristic.WriteValueAsync(buffer.AsBuffer()).AsTask());
            if (result != GattCommunicationStatus.Success)
            {
                LoggerHelper.Instance.Debug("DistanceColorSensor::SetNotifications - failed to subscribe to notifications exception");
                throw new Exception("Failed to subscribe to notifications");
            }
        }

        public event EventHandler<DistanceColorSensorEventArgs> OnChange;
    }
}
