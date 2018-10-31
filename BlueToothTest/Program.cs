using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Devices;
using InTheHand.Devices.Bluetooth;
using InTheHand.Devices.Enumeration;

namespace BlueToothTest
{
    class Program
    {
        static async void ConnectBLEAsync()
        {
            var v = await DeviceInformation.FindAllAsync("*");

            foreach (DeviceInformation di in v)
            {
                Console.WriteLine("ID {0} Name {1}", di.Id, di.Name);
            }

            DeviceInformation di1 = v.First();

            BluetoothDevice bd = await BluetoothDevice.FromIdAsync(di1.Id);
        }

        static void Main(string[] args)
        {
            ConnectBLEAsync();

            Console.ReadLine();
        }
    }
}
