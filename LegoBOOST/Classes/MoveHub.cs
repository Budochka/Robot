using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegoBOOST.Interfaces;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace LegoBOOST.Classes
{
    class MoveHub : IMoveHub
    {
        private GattCharacteristic _characteristic;

        MoveHub(GattCharacteristic gattCharacteristic)
        {
            _characteristic = gattCharacteristic;
        }

        public ILED LED => throw new NotImplementedException();

        public IMotor MotorA => throw new NotImplementedException();

        public IMotor MotorB => throw new NotImplementedException();

        public IMotor MotorAB => throw new NotImplementedException();

        public ITiltSensor TiltSensor => throw new NotImplementedException();

        public IDistanceColorSensor DistanceColorSensor => throw new NotImplementedException();

        private void GetParts()
        {

        }
    }
}
