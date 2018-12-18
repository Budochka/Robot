﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegoBOOST.Interfaces;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;

namespace LegoBOOST.Classes
{
    class MoveHub : IMoveHub
    {
        private GattCharacteristic _characteristic;
        private ILED _led;
        private IMotor _motorA;
        private IMotor _motorB;
        private IMotor _motorAB;
        private ITiltSensor _tiltSensor;
        private IDistanceColorSensor _distanceColorSensor;

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
        }

        public ILED LED { get; }

        public IMotor MotorA { get; }

        public IMotor MotorB { get; }

        public IMotor MotorAB { get; }

        public ITiltSensor TiltSensor { get; }

        public IDistanceColorSensor DistanceColorSensor { get; }

        private void CreateParts()
        {
            _motorA = new Motor(_characteristic, Ports.PORT_A);
            _motorB = new Motor(_characteristic, Ports.PORT_B);
            _motorAB = new Motor(_characteristic, Ports.PORT_AB);
            _led = new LED(_characteristic, Ports.PORT_LED);
        }
}
}
