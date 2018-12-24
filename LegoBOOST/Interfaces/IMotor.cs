namespace LegoBOOST.Interfaces
{
    public interface IMotor
    {
        int Speed { get; set; }
        void SetSpeedTimed(ushort seconds, int speed);
    }
}