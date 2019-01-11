using System;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public interface ITiltSensor
    {
        //granularity is in degrees! make sense for SENSOR_2_AXIS_PRECISE and SENSOR_3_AXIS_PRECISE
        void SetNotificationMode(SensorMode mode, byte granularity = 1);

        event EventHandler<TiltSensorEventArgs> OnChange;
    }
}