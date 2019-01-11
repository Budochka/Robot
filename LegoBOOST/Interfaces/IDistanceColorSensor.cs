using System;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public interface IDistanceColorSensor
    {
        void SetMode(DistanceColorSensorMods mod);

        event EventHandler<DistanceColorSensorEventArgs> OnChange;
    }
}