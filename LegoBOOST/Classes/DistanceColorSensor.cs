using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

[assembly: InternalsVisibleTo("LegoBOOSTNetTests")]

namespace LegoBOOSTNet.Classes
{
    class DistanceColorSensor : IDistanceColorSensor
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;
        private DistanceColorSensorMods _mode;

        internal void FireEvent(byte[] data)
        {
            DistanceColorSensorEventArgs args = new DistanceColorSensorEventArgs();
            var handler = OnChange;
            if (handler == null)
                return;

            switch (_mode)
            {
                case DistanceColorSensorMods.COLOR_ONLY:
                    args.ColorDetected = (Color)data[4];
                    break;

                case DistanceColorSensorMods.DISTANCE_INCHES:
                case DistanceColorSensorMods.DISTANCE_HOW_CLOSE:
                case DistanceColorSensorMods.DISTANCE_SUBINCH_HOW_CLOSE:
                    args.Distance = data[4];
                    break;

                case DistanceColorSensorMods.COUNT_2INCH:
                    args.Count = data[4];
                    break;

                case DistanceColorSensorMods.COLOR_DISTANCE_FLOAT:
                {
                    args.ColorDetected = (Color)data[4];
                    args.Distance = data[5];
                    args.Partial = data[7] == 0 ? 0 : 1.0 / data[7];
                    break;
                }

                case DistanceColorSensorMods.LUMINOSITY:
                    args.Luminosity = (double) data[4] / 1023.0;
                    break;
            }

            handler.Invoke(this, args);
            LoggerHelper.Instance.Debug("DistanceColorSensor::FireEvent OnChange");
        }

        internal DistanceColorSensor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;

            LoggerHelper.Instance.Debug("DistanceColorSensor constructor called");
        }

        public void SetMode(DistanceColorSensorMods mode)
        {
            _mode = mode;

            //load default method format
            var buffer = ConnectionConstants.CMD_SUBSCRIBE_DISTANCE_COLOR;
            //set the port
            buffer[3] = (byte)_port;
            //set mode
            buffer[4] = (byte)_mode;

            LoggerHelper.Instance.Debug($"DistanceColorSensor::SetMode notification {BitConverter.ToString(buffer)} created");

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
