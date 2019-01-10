using System;
using LegoBOOST.Constants;

namespace LegoBOOST.Interfaces
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