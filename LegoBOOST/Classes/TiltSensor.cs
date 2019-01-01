using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Helpers;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
{
    class TiltSensor : ITiltSensor
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;

        internal TiltSensor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;

            LoggerHelper.Instance.Debug("TiltSensor constructor called");
        }
    }
}
