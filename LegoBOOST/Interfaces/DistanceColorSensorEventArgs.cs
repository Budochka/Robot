using System;
using LegoBOOSTNet.Constants;

namespace LegoBOOSTNet.Interfaces
{
    public class DistanceColorSensorEventArgs : EventArgs
    {
        //where `luminosity` is float value from 0 to 1 for LUMINOSITY mode
        public double Luminosity { get; set; }

        //color as in Color type
        public Color ColorDetected { get; set; }
        //distance in inches
        public int Distance { get; set; }
        //part of last inch. Total distance to object is Distance + Partial
        public double Partial { get; set; }

        //it counts crossing distance ~2 inches in front of sensor (for COUNT_2INCH mode)
        public int Count { get; set; }
    }
}