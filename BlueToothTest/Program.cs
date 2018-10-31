using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using InTheHand.Devices.Bluetooth;
using InTheHand.Devices.Enumeration;
using static System.Console;

namespace BlueToothTest
{
    class Program
    {
        static async void ConnectBluetooth()
        {
            BluetoothLEAdvertisementWatcher BleWatcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            BleWatcher.Received += async (w, btAdv) => {
                var device = await BluetoothLEDevice.FromBluetoothAddressAsync(btAdv.BluetoothAddress);
                if (device != null)
                {
                    WriteLine($"BLEWATCHER Found: {device.Name}");

                    // SERVICES!!
                    var gatt = await device.GetGattServicesAsync();
                    if (gatt != null)
                    {
                        WriteLine(
                            $"{device.Name} Services: {gatt.Services.Count}, {gatt.Status}, {gatt.ProtocolError}");
                    }

                    // CHARACTERISTICS!!
//                var characs = await gatt.Services.Single(s => s.Uuid == SAMPLESERVICEUUID).GetCharacteristicsAsync();
//                var charac = characs.Single(c => c.Uuid == SAMPLECHARACUUID);
//                await charac.WriteValueAsync(SOMEDATA);
                }
            };
            BleWatcher.Start();
        }

        static void Main(string[] args)
        {
            ConnectBluetooth();

            // Close on key press
            ReadLine();
        }
    }
}
/*
namespace BlueToothTest
{
    class Program
    {
        static async void ConnectBLEAsync()
        {
            var v = await DeviceInformation.FindAllAsync("*");

            foreach (DeviceInformation di in v)
            {
                WriteLine("ID {0} Name {1}", di.Id, di.Name);
            }

            DeviceInformation di1 = v.First();

            BluetoothDevice bd = await BluetoothDevice.FromIdAsync(di1.Id);
            WriteLine("Device address {0}", bd.BluetoothAddress);
        }

        static void Main(string[] args)
        {
            ConnectBLEAsync();

            ReadLine();
        }
    }
}
*/