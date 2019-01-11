using System;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public class DistanceColorSensorEventArgs : EventArgs
    {
        public Color ColorDetected { get; set; }
        public int Distance { get; set; }
    }

    public interface IDistanceColorSensor
    {
        event EventHandler<DistanceColorSensorEventArgs> OnChange;
    }
}