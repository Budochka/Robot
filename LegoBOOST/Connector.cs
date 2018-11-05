using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using System.Threading.Tasks;

namespace LegoBOOST
{
    public class Connector
    {
        private const ulong _adreessLEGO = 0x001653A7DE77;
        private BluetoothLEDevice _bluetoothLEDevice;

        public Connector()
        {
            _bluetoothLEDevice = null;
        }

        public bool Connect()
        {
            try
            {
                var device = Task.Run(() => BluetoothLEDevice.FromBluetoothAddressAsync(_adreessLEGO)).Result.GetResults();
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
