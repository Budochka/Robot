using System;

namespace LegoBOOSTNet.Interfaces
{
    public interface IMotor
    {
        void SetSpeedTimed(ushort seconds, int speed);
        void SetSpeedTimedAsync(ushort milliseconds, int speed);

        event EventHandler OnActionStart;
        event EventHandler OnActionFinished;
    }
}