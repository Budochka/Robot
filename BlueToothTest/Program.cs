using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using static System.Console;

namespace BlueToothTest
{
    class Program
    {
        private const ulong AdreessLEGO = 0x1653A7DE77;

        static async void ConnectBluetooth()
        {
            //            BluetoothLEAdvertisementWatcher BleWatcher = new BluetoothLEAdvertisementWatcher
            //            {
            //                ScanningMode = BluetoothLEScanningMode.Active
            //            };
            //
            //            BleWatcher.Received += async (w, btAdv) => {
            //                var device = await BluetoothLEDevice.FromBluetoothAddressAsync(btAdv.BluetoothAddress);
                var device = await BluetoothLEDevice.FromBluetoothAddressAsync(AdreessLEGO);
                if (device != null)
                {
                    WriteLine($"BLEWATCHER Found: Name = {device.Name}, Address = {device.BluetoothAddress}, AddressType = {device.BluetoothAddressType}");

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
//            };
//            BleWatcher.Start();
        }

        static void Main(string[] args)
        {
            ConnectBluetooth();

            // Close on key press
            ReadLine();
        }
    }
}
