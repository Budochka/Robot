using System;

namespace LegoBOOSTNet.Interfaces
{
    public interface IMotor
    {
        //for switching motor(s) for x ms with specified dutyCycle (-127, 127). -/+ specifies direction
        void SetSpeedTimed(ushort milliseconds, sbyte dutyCycle1 = 0x64, sbyte dutyCycle2 = 0x00);
        void SetSpeedTimedAsync(ushort milliseconds, sbyte dutyCycle1 = 0x64, sbyte dutyCycle2 = 0x00);

        //for switching motor(s) for angle (0-360) with specified dutyCycle (-127, 127). -/+ specifies direction
        void SetSpeedAngled(uint angle, sbyte dutyCycle1 = 0x64, sbyte dutyCycle2 = 0x00);
        void SetSpeedAngledAsync(uint angle, sbyte dutyCycle1 = 0x64, sbyte dutyCycle2 = 0x00);

        //for switching motor(s) on for constant speed dutyCycle (-127, 127). -/+ specifies direction
        void SetSpeed(sbyte dutyCycle1 = 0x64, sbyte dutyCycle2 = 0x00);
        void SetSpeedAsync(sbyte dutyCycle1 = 0x64, sbyte dutyCycle2 = 0x00);

        event EventHandler OnActionStart;
        event EventHandler OnActionFinished;
    }
}