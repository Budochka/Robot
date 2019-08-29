using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

[assembly: InternalsVisibleTo("LegoBOOSTNetTests")]

namespace LegoBOOSTNet.Classes
{
    class MoveHub : IMoveHub
    {
        private readonly IConnection _connection;
        private LED _led;
        private Motor _motorA;
        private Motor _motorB;
        private Motor _motorAB;
        private Motor _motor3;
        private TiltSensor _tiltSensor;
        private DistanceColorSensor _distanceColorSensor;
        private Button _button;

        private readonly Ports _thirdMotorPort;
        private readonly Ports _distanceColorSensorPort;

        internal MoveHub(IConnection connection, Ports thirdMotor, Ports distanceColorSensor)
        {
            _connection = connection;
            _thirdMotorPort = thirdMotor;
            _distanceColorSensorPort = distanceColorSensor;

            CreateParts(thirdMotor, distanceColorSensor);

            LED = _led;
            MotorA = _motorA;
            MotorB = _motorB;
            MotorAB = _motorAB;
            Motor3 = _motor3;
            TiltSensor = _tiltSensor;
            DistanceColorSensor = _distanceColorSensor;
            Button = _button;

            LoggerHelper.Instance.Debug("MotorHub constructor called");
        }

        public ILED LED { get; }

        public IMotor MotorA { get; }

        public IMotor MotorB { get; }

        public IMotor MotorAB { get; }

        public IMotor Motor3 { get; }

        public ITiltSensor TiltSensor { get; }

        public IDistanceColorSensor DistanceColorSensor { get; }

        public IButton Button { get; }

        private void CreateParts(Ports thirdMotor, Ports distanceColorSensor)
        {
            _motorA = new Motor(_connection, Ports.PORT_A);
            _motorB = new Motor(_connection, Ports.PORT_B);
            _motorAB = new Motor(_connection, Ports.PORT_AB);
            _led = new LED(_connection, Ports.PORT_LED);
            _tiltSensor = new TiltSensor(_connection, Ports.PORT_TILT_SENSOR);
            _button = new Button(_connection);

            //creating devices with variable port numbers
            _motor3 = new Motor(_connection, thirdMotor);
            _distanceColorSensor = new DistanceColorSensor(_connection, distanceColorSensor);

            LoggerHelper.Instance.Debug("MotorHub::CreateParts called");
        }

        private void HandleDeviceInfo(byte[] data)
        {
            LoggerHelper.Instance.Debug($"MotorHub::HandleDeviceInfo event {BitConverter.ToString(data)}");

            if (data.SequenceEqual(ConnectionConstants.EVENT_BUTTON_PRESSED))
                _button.FireEvent(ButtonStatus.BUTTON_PRESSED);
            else if (data.SequenceEqual(ConnectionConstants.EVENT_BUTTON_RELEASED))
                _button.FireEvent(ButtonStatus.BUTTON_RELEASED);
        }

        private void HandleShutDown()
        {
            //unsubscribe from events
            _connection.OnChange -= Connection_ValueChanged;

            //we should close everything, because we are shutting down!
            _led = null;
            _motorA = null;
            _motorB = null;
            _motorAB = null;
            _motor3 = null;
            _tiltSensor = null;
            _distanceColorSensor = null;
            _button = null;
        }

        private void HandlePortInfo(byte[] data)
        {
            switch ((DeviceTypes)data[5])
            {
                case DeviceTypes.DEV_DCS:
                {
                    break;
                }
            }
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
            if ((Ports)data[3] == _distanceColorSensorPort)
                _distanceColorSensor.FireEvent(data);
        }

        private void HandlePortStatus(byte[] data)
        {
            switch ((Ports)data[3])
            {
                case Ports.PORT_LED:
                {
                    if ((ActionStatuses)data[4] == ActionStatuses.STATUS_FINISHED)
                    {
                        _led?.FireOnColorChanged();
                        LoggerHelper.Instance.Debug("MotorHub::HandlePortStatus LED color changed");
                    }
                    break;
                }

                case Ports.PORT_A:
                {
                    _motorA?.FireEvent(data[4]);
                    LoggerHelper.Instance.Debug("MotorHub::HandlePortStatus motor A event fired");
                    break;
                }

                case Ports.PORT_B:
                {
                    _motorB?.FireEvent(data[4]);
                    LoggerHelper.Instance.Debug("MotorHub::HandlePortStatus motor B event fired");
                    break;
                }

                case Ports.PORT_AB:
                {
                    _motorAB?.FireEvent(data[4]);
                    LoggerHelper.Instance.Debug("MotorHub::HandlePortStatus motor AB event fired");
                    break;
                }
            }
        }

        private void Connection_ValueChanged(Object sender, NotificationEventArgs args)
        {
            //we should handle only our messages
            if (sender != _connection)
                return;

            LoggerHelper.Instance.Debug($"MotorHub::DataCharacteristic_ValueChanged notification received {BitConverter.ToString(args.Data)}");

            int length = args.Data[0]; //message length

            switch ((PacketType)args.Data[2])
            {
                //device information
                case PacketType.MSG_DEVICE_INFO:
                {
                    HandleDeviceInfo(args.Data);
                    break;
                }

                //device shutdown
                case PacketType.MSG_DEVICE_SHUTDOWN:
                {
                    HandleShutDown();
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
                    HandlePortInfo(args.Data);
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
                    HandleSensorData(args.Data);
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
                    HandlePortStatus(args.Data);
                    break;
                }
            }
        }
    }
}
