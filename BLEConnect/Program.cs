using System;
using Windows.Devices.Bluetooth.Advertisement;
using static System.Console;

namespace BLEConnect
{
    class Program
    {
        static private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // Tell the user we see an advertisement and print some properties
            WriteLine(String.Format("Advertisement:"));
            WriteLine(String.Format("  BT_ADDR: {0}", eventArgs.BluetoothAddress));
            WriteLine(String.Format("  FR_NAME: {0}", eventArgs.Advertisement.LocalName));
            WriteLine();
        }

        static void Main(string[] args)
        {
            var watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active,
                SignalStrengthFilter =
                {
                    // Only activate the watcher when we're recieving values >= -80
                    InRangeThresholdInDBm = -80,
                    // Stop watching if the value drops below -90 (user walked away)
                    OutOfRangeThresholdInDBm = -90
                }
            };

            // Register callback for when we see an advertisements
            watcher.Received += OnAdvertisementReceived;

            // Wait 5 seconds to make sure the device is really out of range
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            // Starting watching for advertisements
            watcher.Start();

            // Close on key press
            ReadLine();
        }
    }
}
