using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Helpers;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
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

        public void SetNotificationMode(SensorMode mode)
        {
            _mode = mode;


        }

        internal void FireEvent(byte[] data)
        {
            var handler = OnChange;
            if (handler == null)
                return;
            switch (_mode)
            {
                case SensorMode.SENSOR_2_AXIS_PRECISE:
                    break;
                case SensorMode.SENSOR_2_AXIS_SIMPLE:
                    break;
                case SensorMode.SENSOR_3_AXIS_SIMPLE:
                    break;
                case SensorMode.SENSOR_BUMP_DETECT:
                    break;
                case SensorMode.SENSOR_3_AXIS_PRECISE:
                    break;
            }
            TiltSensorEventArgs args = new TiltSensorEventArgs() { Pitch = 1, Yaw= 1, Roll = 1 };
            handler.Invoke(this, args);
            LoggerHelper.Instance.Debug("Motor::FireEvent ActionStart");
        }

        public event EventHandler<TiltSensorEventArgs> OnChange;
    }
}
