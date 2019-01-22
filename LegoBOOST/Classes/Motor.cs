using System;
using System.CodeDom;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;

[assembly: InternalsVisibleTo("LegoBOOSTNetTests")]

namespace LegoBOOSTNet.Classes
{
    class Motor : IMotor
    {
        private readonly IConnection _connection;
        private readonly Ports _port;

        internal Motor(IConnection connection, Ports port)
        {
            _connection = connection;
            _port = port;

            LoggerHelper.Instance.Debug("Motor constructor called");
        }

        internal void FireEvent(byte data)
        {
            switch ((ActionStatuses)data)
            {
                case ActionStatuses.STATUS_STARTED:
                {
                    EventHandler handler = OnActionStart;
                    handler?.Invoke(this, EventArgs.Empty);
                    LoggerHelper.Instance.Debug("Motor::FireEvent ActionStart");
                    break;
                }

                case ActionStatuses.STATUS_FINISHED:
                {
                    EventHandler handler = OnActionFinished;
                    handler?.Invoke(this, EventArgs.Empty);
                    LoggerHelper.Instance.Debug("Motor::FireEvent ActionFinished");
                    break;
                }
            }
        }

        public async void SetSpeedTimedAsync(ushort milliseconds, sbyte dutyCycle = 0x64)
        {
            await _connection.WriteValueAsync(CreateMessageTimed(milliseconds, (byte)dutyCycle));

            LoggerHelper.Instance.Debug($"Motor::SetSpeedTimedAsync milliseconds = {milliseconds}, speed = {dutyCycle}");
        }

        public event EventHandler OnActionStart;
        public event EventHandler OnActionFinished;

        public void SetSpeedTimed(ushort milliseconds, sbyte dutyCycle = 0x64)
        {
            if (!_connection.WriteValue(CreateMessageTimed(milliseconds, (byte)dutyCycle)))
            {
                LoggerHelper.Instance.Debug("Motor::SetSpeedTimed - failed to set speed");
                throw new Exception("Failed to set speed");
            }

            LoggerHelper.Instance.Debug($"Motor::SetSpeedTimed seconds = {milliseconds}, speed = {dutyCycle}");
        }

        //dutyCycle - is value for motor cycle. Not known if it will be used
        private byte[] CreateMessageTimed(ushort milliseconds, byte dutyCycle = 0x64)
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
            byte[] bytes = BitConverter.GetBytes(milliseconds);
            Array.Reverse(bytes);
            message[6] = bytes[0]; 
            message[7] = bytes[1]; 

            message[8] = dutyCycle; 
            Array.Copy(ConnectionConstants.TRAILER, 0, message, 9, ConnectionConstants.TRAILER.Length);

            LoggerHelper.Instance.Debug($"Motor::Message {BitConverter.ToString(message)} created");

            return message;
        }
    }
}
