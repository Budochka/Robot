using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Helpers;
using LegoBOOST.Interfaces;
using LegoBOOST.Constants;

namespace LegoBOOST.Classes
{
    public class Connector
    {
        private BluetoothLEDevice _bluetoothLEDevice;
        private GattDeviceService _deviceService;
        private GattCharacteristic _characteristic;

        public Connector()
        {
            _bluetoothLEDevice = null;
            _deviceService = null;
            _characteristic = null;

            LoggerHelper.Instance.Debug("Connector::Connector called");
        }

        public IMoveHub CreateMoveHub(Ports thirdMotor, Ports distanceColorSensor)
        {
            if (_characteristic == null)
            {
                var s = "Trying to create IMoveHub before connecting to bluetooth device";
                LoggerHelper.Instance.Error(s);
                throw (new Exception(s));
            }
            else
            {
                LoggerHelper.Instance.Debug("Connector::CreateMoveHub called successully");
                return new MoveHub(_characteristic, thirdMotor, distanceColorSensor);
            }
        }

        public bool Connect()
        {
            try
            {
                var device = AsyncHelpers.RunSync<BluetoothLEDevice>(() => BluetoothLEDevice.FromBluetoothAddressAsync(ConnectionConstants.AdreessLEGO).AsTask());
                if (device != null)
                {
                    _bluetoothLEDevice = device;
                    var services = AsyncHelpers.RunSync<GattDeviceServicesResult>(() 
                        => device.GetGattServicesForUuidAsync(Guid.Parse(ConnectionConstants.ServiceUUID)).AsTask());
                    if (services.Services.Count > 0)
                    {
                        _deviceService = services.Services[0];
                        var characteristics = AsyncHelpers.RunSync<GattCharacteristicsResult>(() 
                            => _deviceService.GetCharacteristicsForUuidAsync(Guid.Parse(ConnectionConstants.CharacteristicUUID)).AsTask());
                        if (characteristics.Characteristics.Count > 0)
                        {
                            _characteristic = characteristics.Characteristics[0];
                            GattCharacteristicProperties properties = _characteristic.CharacteristicProperties;
                            LoggerHelper.Instance.Debug("Characteristic properties:");
                            foreach (GattCharacteristicProperties property in Enum.GetValues(typeof(GattCharacteristicProperties)))
                            {
                                if (properties.HasFlag(property))
                                {
                                    LoggerHelper.Instance.Debug($"{property} ");
                                }
                            }
                            LoggerHelper.Instance.Debug("Connector::Connect characteristic created");
                            return true;
                        }
                        else
                        {
                            LoggerHelper.Instance.Debug("Connector::Connect characteristic not found");
                            return false;
                        }
                    }
                    else
                    {
                        LoggerHelper.Instance.Debug("Connector::Connect service not found");
                        return false;
                    }
                }
                else
                {
                    LoggerHelper.Instance.Debug("Connector::Connect device not found");
                    return false;
                }
            }
            catch (Exception e)
            {
                //0x800710df - ERROR_DEVICE_NOT_AVAILABLE because the Bluetooth radio is not on
                if ((uint)e.HResult == 0x800710df)
                {
                    var s = "ERROR_DEVICE_NOT_AVAILABLE because the Bluetooth radio is not on";
                    LoggerHelper.Instance.Error(s);
                    throw (new Exception(s, e));
                }
                throw;
            }
        }

        public void Disconnect()
        {
            _bluetoothLEDevice?.Dispose();
            _bluetoothLEDevice = null;
            _deviceService = null;
            _characteristic = null;

            LoggerHelper.Instance.Debug("Connector:Disconnect called");
        }
    }
}
