using System;
using System.Collections.Generic;
using System.Text;

namespace LegoBOOST.Interfaces
{
    public interface IMoveHub
    {
        ILED LED { get; }
        IMotor MotorA { get; }
        IMotor MotorB { get; }
        IMotor MotorAB { get; }
        ITiltSensor TiltSensor { get; }
        IDistanceColorSensor DistanceColorSensor { get; }
    }
}
