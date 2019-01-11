namespace LegoBOOSTNet.Interfaces
{
    public interface IMoveHub
    {
        ILED LED { get; }
        IMotor MotorA { get; }
        IMotor MotorB { get; }
        IMotor MotorAB { get; }
        IMotor Motor3 { get; }
        ITiltSensor TiltSensor { get; }
        IDistanceColorSensor DistanceColorSensor { get; }
        IButton Button { get; }
    }
}
