using System;
using LegoBOOST.Constants;

namespace LegoBOOST.Interfaces
{
    public class SensorEventArgs : EventArgs
    {
        public Color ColorDetected { get; set; }
        public int Distance { get; set; }
    }

    public interface IDistanceColorSensor
    {
        event EventHandler<SensorEventArgs> OnChange;
    }
}