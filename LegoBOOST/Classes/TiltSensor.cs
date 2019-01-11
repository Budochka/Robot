using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

namespace LegoBOOSTNet.Classes
{
    class TiltSensor : ITiltSensor
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;
        private SensorMode _mode;

        internal TiltSensor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;

            LoggerHelper.Instance.Debug("TiltSensor constructor called");
        }

        public void SetNotificationMode(SensorMode mode, byte granularity = 1)
        {
            _mode = mode;

            var buffer = ConnectionConstants.CMD_SUBSCRIBE_TILT_SENSOR;
            buffer[4] = (byte)mode;
            buffer[5] = granularity;

            AsyncHelpers.RunSync<GattCommunicationStatus>(() => _characteristic.WriteValueAsync(buffer.AsBuffer()).AsTask());

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
