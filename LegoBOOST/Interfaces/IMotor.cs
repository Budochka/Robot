namespace LegoBOOST.Interfaces
{
    public interface IMotor
    {
        int Speed { get; set; }
        void SetSpeedTimed(ushort seconds, int speed);
        void SetSpeedTimedAsync(ushort seconds, int speed);
    }
}