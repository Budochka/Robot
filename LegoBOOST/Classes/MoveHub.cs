using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegoBOOST.Interfaces;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using LegoBOOST.Constants;

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

            _characteristic.ValueChanged += dataCharacteristic_ValueChanged;
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
        }

        private void dataCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            //we should handle only our messages
            if (sender != _characteristic)
                return;

            byte[] data = new byte[args.CharacteristicValue.Length];
            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);

            int length = data[0]; //message length

            switch (data[2])
            {
                //device information
                case 0x01:
                {
                    break;
                }

                //device shutdown
                case 0x02:
                {
                    break;
                }

                //? ping response
                case 0x03:
                {
                    break;
                }

                //port information
                case 0x04:
                {
                    break;
                }

                //error notification
                case 0x05:
                {
                    break;
                }

                //subscription
                case 0x41:
                {
                    break;
                }

                //sensor reading
                case 0x45:
                {
                    break;
                }

                //subscription acknowledgement
                case 0x47:
                {
                    break;
                }

                //port changes
                case 0x82:
                {
                    break;
                }
            }
        }
    }
}
