using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Storage.Streams;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

[assembly: InternalsVisibleTo("LegoBOOSTNetTests")]

namespace LegoBOOSTNet.Classes
{
    public class Connector : IConnection
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
            return new MoveHub(this, thirdMotor, distanceColorSensor);
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

        //connect to bluetooth device
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

                    //get the Lego Boost service
                    var services = AsyncHelpers.RunSync(() 
                        => device.GetGattServicesForUuidAsync(Guid.Parse(ConnectionConstants.ServiceUUID)).AsTask());
                    if (services.Services.Count > 0)
                    {
                        //there is should be only one service with such UUID
                        _deviceService = services.Services[0];
                        //get the characteristic
                        var characteristics = AsyncHelpers.RunSync(() 
                            => _deviceService.GetCharacteristicsForUuidAsync(Guid.Parse(ConnectionConstants.CharacteristicUUID)).AsTask());
                        if (characteristics.Characteristics.Count > 0)
                        {
                            //there is should be only one characteristic with such UUID
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

                            //subscribe to the GATT characteristic notification
                            var status = AsyncHelpers.RunSync(() =>
                                _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify).AsTask());

                            if (status == GattCommunicationStatus.Success)
                            {
                                LoggerHelper.Instance.Debug("Subscribing to the Indication/Notification");
                                _characteristic.ValueChanged += DataCharacteristic_ValueChanged;
                            }
                            else
                            {
                                LoggerHelper.Instance.Debug($"Connetor::Connect set notification failed: {status}");
                                throw new Exception("Connetor::Connect set notification failed");
                            }

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

        private void DataCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            //we should handle only our messages
            if (sender != _characteristic)
                return;

            var handler = OnChange;

            if (handler != null)
            {
                byte[] data = new byte[args.CharacteristicValue.Length];
                DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);

                var notificationArgs = new NotificationEventArgs { Data = data.ToArray() };
                handler.Invoke(this, notificationArgs);

                LoggerHelper.Instance.Debug($"Connector::DataCharacteristic_ValueChanged message {BitConverter.ToString(data)}");
            }
        }


        public void Disconnect()
        {
            _characteristic.ValueChanged -= DataCharacteristic_ValueChanged;

            _bluetoothLEDevice?.Dispose();
            _bluetoothLEDevice = null;
            _deviceService = null;
            _characteristic = null;

            LoggerHelper.Instance.Debug("Connector:Disconnect called");
        }

        public bool WriteValue(byte[] data)
        {
            var result = AsyncHelpers.RunSync(() => _characteristic.WriteValueAsync(data.AsBuffer()).AsTask());
            return (result == GattCommunicationStatus.Success);
        }

        public async Task<bool> WriteValueAsync(byte[] data)
        {
            var result = await _characteristic.WriteValueAsync(data.AsBuffer());
            return (result == GattCommunicationStatus.Success);
        }

        public event EventHandler<NotificationEventArgs> OnChange;
    }
}
