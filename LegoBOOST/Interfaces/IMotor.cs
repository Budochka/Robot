using System;

namespace LegoBOOST.Interfaces
{
    public interface IMotor
    {
        void SetSpeedTimed(ushort seconds, int speed);
        void SetSpeedTimedAsync(ushort seconds, int speed);

        event EventHandler OnActionStart;
        event EventHandler OnActionFinished;
    }
}