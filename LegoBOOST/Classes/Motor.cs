using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using LegoBOOST.Constants;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
{
    class Motor : IMotor
    {
        private readonly GattCharacteristic _characteristic;
        private readonly Ports _port;

        internal Motor(GattCharacteristic characteristic, Ports port)
        {
            _characteristic = characteristic;
            _port = port;
        }

        public int Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async void SetSpeedTimed(ushort seconds, int speed)
        {
            if (Math.Abs(speed) > 100)
            {
                throw (new Exception("Speed value should be between -100 and 100"));
            }

            var bytes = PackMessageTimed(seconds, speed);
            var buffer = bytes.AsBuffer();

            await _characteristic.WriteValueAsync(buffer);
        }

        //dutyCycle - is value for motor cycle. Not known if it will be used
        private byte[] PackMessageTimed(ushort seconds, int speed, byte dutyCycle = 0x64)
        {
            //message to be sent to motor
            var message = new byte[12]; //12 is packed length for timed values
            message[0] = (byte) message.Length;
            message[1] = 0; //by default
            message[2] = 0x81; //by default for messages sent to some port
            message[3] = (byte) _port;
            message[4] = 0x11; //by default
            message[5] = 0x09; //Time Value

            //convert seconds to little endian
            byte[] bytes = BitConverter.GetBytes(seconds);
            Array.Reverse(bytes);
            message[6] = bytes[0]; 
            message[7] = bytes[1]; 

            message[8] = dutyCycle; 
            Array.Copy(ConnectionConstants.TRAILER, 0, message, 9, ConnectionConstants.TRAILER.Length);

            return message;
        }
    }
}
