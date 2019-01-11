using System;

namespace LegoBOOSTNet.Interfaces
{
    public interface IDistanceColorSensor
    {
        event EventHandler<DistanceColorSensorEventArgs> OnChange;
    }
}