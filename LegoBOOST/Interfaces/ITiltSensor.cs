using System;
using LegoBOOST.Constants;

namespace LegoBOOST.Interfaces
{
    public class TiltSensorEventArgs : EventArgs
    {
        public int Pitch { get; set; }
        public int Yaw { get; set; }
        public int Roll { get; set; }
    }

    public interface ITiltSensor
    {
        void SetNotificationMode(SensorMode mode);

        event EventHandler<TiltSensorEventArgs> OnChange;
    }
}