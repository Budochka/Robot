using LegoBOOST.Classes;
using LegoBOOST.Constants;
using static System.Console;

namespace BlueToothTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var connector = new Connector();

            if (!connector.Connect())
                return;

            var movehub = connector.CreateMoveHub(Ports.PORT_D, Ports.PORT_C);
            var motor = movehub.Motor3;
//            motor.SetSpeedTimedAsync(10, 100);

            var LED = movehub.LED;
//            LED.SetColor(Color.Red);

            // Close on key press
            ReadLine();
            connector.Disconnect();
        }
    }
}
