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
        private readonly IConnection _connection;
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

        internal DistanceColorSensor(IConnection connection, Ports port)
        {
            _connection = connection;
            _port = port;
            _mode = DistanceColorSensorMods.NOT_SET;

            LoggerHelper.Instance.Debug("DistanceColorSensor constructor called");
        }

        private bool Subscribe(byte mode, bool subscribe)
        {
            var buffer = (byte[])ConnectionConstants.CMD_SUBSCRIBE_DISTANCE_COLOR.Clone();

            //set the port
            buffer[3] = (byte)_port;
            //set the mode
            buffer[4] = mode;
            //01 - subscribe, 00 - unsubscribe
            buffer[9] = (byte) (subscribe ? 0x01 : 0x00);

            LoggerHelper.Instance.Debug($"DistanceColorSensor::SetMode notification {BitConverter.ToString(buffer)} created");

            return _connection.WriteValue(buffer);
        }

        public void SetMode(DistanceColorSensorMods mode)
        {
            if (_mode == mode)
            {
                LoggerHelper.Instance.Debug("DistanceColorSensor::SetMode no mode change");
                return;
            }

            if (_mode != DistanceColorSensorMods.NOT_SET)
            {
                if (!Subscribe((byte)_mode, false))
                {
                    LoggerHelper.Instance.Debug("DistanceColorSensor::SetNotifications - failed to unsubscribe from notifications exception");
                    throw new Exception("Failed to unsubscribe from notifications");
                }
            }

            _mode = mode;

            if (!Subscribe((byte)_mode, false))
            {
                LoggerHelper.Instance.Debug("DistanceColorSensor::SetNotifications - failed to subscribe to notifications exception");
                throw new Exception("Failed to subscribe to notifications");
            }

            LoggerHelper.Instance.Debug($"DistanceColorSensor::SetNotificationMode mode = {mode}");
        }

        public event EventHandler<DistanceColorSensorEventArgs> OnChange;
    }
}
