using System;
using LegoBOOST;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Classes;
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

            var moveHub = connector.CreateMoveHub();
            var motorA = moveHub?.MotorA;
            motorA?.SetSpeedTimed(10, 100);

            var motorB = moveHub?.MotorB;
            motorB?.SetSpeedTimed(10, 100);

            // Close on key press
            ReadLine();
            connector.Disconnect();
        }
    }
}
