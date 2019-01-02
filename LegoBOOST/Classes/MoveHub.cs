using System;
using LegoBOOST.Interfaces;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using LegoBOOST.Constants;
using LegoBOOST.Helpers;

namespace LegoBOOST.Classes
{
    class MoveHub : IMoveHub
    {
        private readonly GattCharacteristic _characteristic;
        private ILED _led;
        private IMotor _motorA;
        private IMotor _motorB;
        private IMotor _motorAB;
        private ITiltSensor _tiltSensor;
        private IDistanceColorSensor _distanceColorSensor;
        private IButton _button;

        internal MoveHub(GattCharacteristic gattCharacteristic)
        {
            _characteristic = gattCharacteristic;

            CreateParts();

            LED = _led;
            MotorA = _motorA;
            MotorB = _motorB;
            MotorAB = _motorAB;
            TiltSensor = _tiltSensor;
            DistanceColorSensor = _distanceColorSensor;
            Button = _button;

            //subscribe to the GATT characteristic notification
            GattCommunicationStatus status = AsyncHelpers.RunSync<GattCommunicationStatus>(()
                =>_characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify).AsTask());

            if (status == GattCommunicationStatus.Success)
            {
                LoggerHelper.Instance.Debug("Subscribing to the Indication/Notification");
                _characteristic.ValueChanged += DataCharacteristic_ValueChanged;
            }
            else
            {
                LoggerHelper.Instance.Debug("MoveHub::MoveHub set notification failed: {status}");
            }

            LoggerHelper.Instance.Debug("MotorHub constructor called");
        }

        public ILED LED { get; }

        public IMotor MotorA { get; }

        public IMotor MotorB { get; }

        public IMotor MotorAB { get; }

        public ITiltSensor TiltSensor { get; }

        public IDistanceColorSensor DistanceColorSensor { get; }

        public IButton Button { get; }

        private void CreateParts()
        {
            _motorA = new Motor(_characteristic, Ports.PORT_A);
            _motorB = new Motor(_characteristic, Ports.PORT_B);
            _motorAB = new Motor(_characteristic, Ports.PORT_AB);
            _led = new LED(_characteristic, Ports.PORT_LED);
            _tiltSensor = new TiltSensor(_characteristic, Ports.PORT_TILT_SENSOR);
            //Need yo understand what port to use
            _distanceColorSensor = new DistanceColorSensor(_characteristic, 0);

            LoggerHelper.Instance.Debug("MotorHub::CreateParts called");
        }

        private void HandleDeviceInfo(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void HandleShutDown()
        {
            throw new NotImplementedException();
        }

        private void HandlePortInfo(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void HandlePortCmdError(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void HandleSensorSubscribeAck(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void HandleSensorData(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void HandlePortStatus(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void DataCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            //we should handle only our messages
            if (sender != _characteristic)
                return;

            byte[] data = new byte[args.CharacteristicValue.Length];
            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);

            LoggerHelper.Instance.Debug($"MotorHub::DataCharacteristic_ValueChanged notification received {BitConverter.ToString(data)}");

            int length = data[0]; //message length

            switch ((PacketType)data[2])
            {
                //device information
                case PacketType.MSG_DEVICE_INFO:
                {
                    //HandleDeviceInfo(data);
                    break;
                }

                //device shutdown
                case PacketType.MSG_DEVICE_SHUTDOWN:
                {
                    //HandleShutDown();
                    break;
                }

                //? ping response
                case PacketType.MSG_PING_RESPONSE:
                {
                    break;
                }

                //port information
                case PacketType.MSG_PORT_INFO:
                {
                    //HandlePortInfo(data);
                    break;
                }

                //error notification
                case PacketType.MSG_PORT_CMD_ERROR:
                {
                    //HandlePortCmdError(data);
                    break;
                }

                //subscription
                case PacketType.MSG_SENSOR_SUBSCRIBE:
                {
                    break;
                }

                //sensor reading
                case PacketType.MSG_SENSOR_DATA:
                {
                    //HandleSensorData(data);
                    break;
                }

                //subscription acknowledgement
                case PacketType.MSG_SENSOR_SUBSCRIBE_ACK:
                {
                    //HandleSensorSubscribeAck(data);
                    break;
                }

                //port changes
                case PacketType.MSG_PORT_STATUS:
                {
                    //HandlePortStatus(data);
                    break;
                }
            }
        }
    }
}
