using System;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public class TiltSensorEventArgs : EventArgs
    {
        //valid for bump data and simple mods
        public int State { get; set; }

        //for precise mods (depends on mode 2D or 3D)
        public int Pitch { get; set; }
        public int Yaw { get; set; }
        public int Roll { get; set; }
    }

    public interface ITiltSensor
    {
        //granularity is in degrees! make sense for SENSOR_2_AXIS_PRECISE and SENSOR_3_AXIS_PRECISE
        void SetNotificationMode(SensorMode mode, byte granularity = 1);

        event EventHandler<TiltSensorEventArgs> OnChange;
    }
}