using System;
using Windows.Devices.Bluetooth;
using LegoBOOST.Helpers;


namespace LegoBOOST.Classes
{
    public class Connector
    {
        private const ulong _adreessLEGO = 0x001653A7DE77;
        private const string _serviceUUID = "00001624-1212-efde-1623-785feabcd123";
        private BluetoothLEDevice _bluetoothLEDevice;

        public Connector()
        {
            _bluetoothLEDevice = null;
        }

        public bool Connect()
        {
            try
            {
                var device = AsyncHelpers.RunSync<BluetoothLEDevice>(() => BluetoothLEDevice.FromBluetoothAddressAsync(_adreessLEGO).AsTask());
                if (device != null)
                {
                    _bluetoothLEDevice = device;
                    return true;
                }
            }
            catch (Exception e)
            {
                //0x800710df - ERROR_DEVICE_NOT_AVAILABLE because the Bluetooth radio is not on
                if (e.HResult == 0x800710df)
                {
                    throw (new Exception("ERROR_DEVICE_NOT_AVAILABLE because the Bluetooth radio is not on", e));
                }
                throw;
            }

            return false;
        }

        public void Disconnect()
        {
            _bluetoothLEDevice?.Dispose();
            _bluetoothLEDevice = null;
        }
    }
}
