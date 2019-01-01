using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Interfaces;
using NLog;

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

            LogManager.GetCurrentClassLogger().Debug("TiltSensor constructor called");
        }
    }
}
