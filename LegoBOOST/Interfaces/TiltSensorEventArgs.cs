using System;

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
}