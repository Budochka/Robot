namespace LegoBOOST.Interfaces
{
    public interface IMotor
    {
        int Speed { get; set; }
        void SetSpeedTimed(uint seconds, int speed);
    }
}