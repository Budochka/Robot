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
    class TiltSensor : ITiltSensor
    {
        private readonly IConnection _connection;
        private readonly Ports _port;
        private SensorMode _mode;

        internal TiltSensor(IConnection connection, Ports port)
        {
            _connection = connection;
            _port = port;
            _mode = SensorMode.NOT_SET;

            LoggerHelper.Instance.Debug("TiltSensor constructor called");
        }

        private bool Subscribe(byte mode, byte granularity, bool subscribe)
        {
            var buffer = (byte[])ConnectionConstants.CMD_SUBSCRIBE_TILT_SENSOR.Clone();

            //set the mode
            buffer[4] = mode;
            //set notifications granularity
            buffer[5] = granularity;
            //01 - subscribe, 00 - unsubscribe
            buffer[9] = (byte)(subscribe ? 0x01 : 0x00);

            LoggerHelper.Instance.Debug($"TiltSensor::SetMode notification {BitConverter.ToString(buffer)} created");

            return _connection.WriteValue(buffer);
        }

        public void SetNotificationMode(SensorMode mode, byte granularity = 1)
        {
            if (_mode == mode)
            {
                LoggerHelper.Instance.Debug("TiltSensor::SetMode no mode change");
                return;
            }

            if (_mode != SensorMode.NOT_SET)
            {
                if (!Subscribe((byte)_mode, granularity, false))
                {
                    LoggerHelper.Instance.Debug("TiltSensor::SetNotifications - failed to unsubscribe from notifications exception");
                    throw new Exception("Failed to unsubscribe from notifications");
                }
            }

            _mode = mode;

            if (!Subscribe((byte)_mode, granularity, true))
            {
                LoggerHelper.Instance.Debug("TiltSensor::SetNotificationMode - failed to subscribe to notifications");
                throw new Exception("Failed to subscribe to notifications");
            }

            LoggerHelper.Instance.Debug($"TiltSensor::SetNotificationMode mode = {mode}, granularity = {granularity}");
        }

        internal void FireEvent(byte[] data)
        {
            var handler = OnChange;
            if (handler == null)
                return;

            TiltSensorEventArgs args = new TiltSensorEventArgs();

            switch (_mode)
            {
                case SensorMode.SENSOR_2_AXIS_SIMPLE:
                case SensorMode.SENSOR_3_AXIS_SIMPLE:
                case SensorMode.SENSOR_BUMP_DETECT:
                {
                    args.State = data[4];
                    break;
                }

                case SensorMode.SENSOR_2_AXIS_PRECISE:
                {
                    args.Roll = data[4];
                    args.Pitch = data[5];
                    break;
                }

                case SensorMode.SENSOR_3_AXIS_PRECISE:
                {
                    args.Roll = data[4];
                    args.Pitch = data[5];
                    args.Yaw = data[6];
                    break;
                }
            }

            handler.Invoke(this, args);
            LoggerHelper.Instance.Debug($"TiltSensor::FireEvent {BitConverter.ToString(data)}");
        }

        public event EventHandler<TiltSensorEventArgs> OnChange;
    }
}
