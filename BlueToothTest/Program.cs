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

            var movehub = connector.CreateMoveHub();
            var motor = movehub.MotorAB;

            // Close on key press
            ReadLine();
            connector.Disconnect();
        }
    }
}
