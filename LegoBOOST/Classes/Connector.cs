using System;
using System.Runtime.CompilerServices;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

[assembly: InternalsVisibleTo("LegoBOOSTNetTests")]

namespace LegoBOOSTNet.Classes
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

            LoggerHelper.Instance.Debug("Connector::CreateMoveHub called successully");
            return new MoveHub(_characteristic, thirdMotor, distanceColorSensor);
        }

        private void handlerPairingReq(DeviceInformationCustomPairing CP, DevicePairingRequestedEventArgs DPR)
        {
            //so we get here for custom pairing request.
            //this is the magic place where your pin goes.
            //my device actually does not require a pin but
            //windows requires at least a "0".  So this solved 
            //it.  This does not pull up the Windows UI either.
            DPR.Accept("0");
        }

        public bool Connect()
        {
            try
            {
                var device = AsyncHelpers.RunSync(() => BluetoothLEDevice.FromBluetoothAddressAsync(ConnectionConstants.AdreessLEGO).AsTask());
                if (device != null)
                {
                    if (device.DeviceInformation.Pairing.IsPaired == false)
                    {

                        /* Optional Below - Some examples say use FromIdAsync
                        to get the device. I don't think that it matters.   */
                        var did = device.DeviceInformation.Id; //I reuse did to reload later.
                        device.Dispose();
                        device = null;
                        device = AsyncHelpers.RunSync(() => BluetoothLEDevice.FromIdAsync(did).AsTask());
                        /* end optional */
                        var handlerPairingRequested = new TypedEventHandler<DeviceInformationCustomPairing, DevicePairingRequestedEventArgs>(handlerPairingReq);
                        device.DeviceInformation.Pairing.Custom.PairingRequested += handlerPairingRequested;
                        LoggerHelper.Instance.Debug("Paired to device");

                        var prslt = AsyncHelpers.RunSync(() => device.DeviceInformation.Pairing.Custom.PairAsync(DevicePairingKinds.ProvidePin, DevicePairingProtectionLevel.None).AsTask());
                        LoggerHelper.Instance.Debug($"Custom PAIR complete status: {prslt.Status}, Connection Status: {device.ConnectionStatus}");

                        device.DeviceInformation.Pairing.Custom.PairingRequested -= handlerPairingRequested; //Don't need it anymore once paired.

                        if (prslt.Status != DevicePairingResultStatus.Paired)
                        {
                            //This should not happen. If so we exit to try again.
                            LoggerHelper.Instance.Debug("prslt exiting.  prslt.status=" + prslt.Status); // so the status may have updated.  lets drop out of here and get the device again.  should be paired the 2nd time around?
                            return false;
                        }
                    }

                    _bluetoothLEDevice = device;
                    var services = AsyncHelpers.RunSync(() 
                        => device.GetGattServicesForUuidAsync(Guid.Parse(ConnectionConstants.ServiceUUID)).AsTask());
                    if (services.Services.Count > 0)
                    {
                        _deviceService = services.Services[0];
                        var characteristics = AsyncHelpers.RunSync(() 
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

                        LoggerHelper.Instance.Debug("Connector::Connect characteristic not found");
                        return false;
                    }

                    LoggerHelper.Instance.Debug("Connector::Connect service not found");
                    return false;
                }

                LoggerHelper.Instance.Debug("Connector::Connect device not found");
                return false;
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
