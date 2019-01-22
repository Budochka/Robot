using System;

namespace LegoBOOSTNet.Interfaces
{
    public interface IMotor
    {
        void SetSpeedTimed(ushort seconds, sbyte dutyCycle = 0x64);
        void SetSpeedTimedAsync(ushort milliseconds, sbyte dutyCycle = 0x64);

        event EventHandler OnActionStart;
        event EventHandler OnActionFinished;
    }
}