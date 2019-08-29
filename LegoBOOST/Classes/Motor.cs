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

        public void SetSpeedTimed(ushort milliseconds, sbyte dutyCycle1, sbyte dutyCycle2)
        {
            if (!_connection.WriteValue(CreateMessageTimed(milliseconds, (byte)dutyCycle1, (byte)dutyCycle2)))
            {
                LoggerHelper.Instance.Debug("Motor::SetSpeedTimed - failed to set speed");
                throw new Exception("Failed to set speed");
            }

            LoggerHelper.Instance.Debug($"Motor::SetSpeedTimedAsync milliseconds = {milliseconds}, speed = {dutyCycle1}, {dutyCycle2}");
        }

        public async void SetSpeedTimedAsync(ushort milliseconds, sbyte dutyCycle1, sbyte dutyCycle2)
        {
            await _connection.WriteValueAsync(CreateMessageTimed(milliseconds, (byte)dutyCycle1, (byte)dutyCycle2));

            LoggerHelper.Instance.Debug($"Motor::SetSpeedTimedAsync milliseconds = {milliseconds}, speed = {dutyCycle1}, {dutyCycle2}");
        }

        public void SetSpeedAngled(uint angle, sbyte dutyCycle1, sbyte dutyCycle2)
        {
            if (!_connection.WriteValue(CreateMessageAngled(angle, (byte)dutyCycle1, (byte)dutyCycle2)))
            {
                LoggerHelper.Instance.Debug("Motor::SetSpeedTimed - failed to set speed");
                throw new Exception("Failed to set speed");
            }

            LoggerHelper.Instance.Debug($"Motor::SetSpeedTimedAsync angle = {angle}, speed = {dutyCycle1}, {dutyCycle2}");
        }

        public async void SetSpeedAngledAsync(uint angle, sbyte dutyCycle1, sbyte dutyCycle2)
        {
            await _connection.WriteValueAsync(CreateMessageAngled(angle, (byte)dutyCycle1, (byte)dutyCycle2));

            LoggerHelper.Instance.Debug($"Motor::SetSpeedTimedAsync angle = {angle}, speed = {dutyCycle1}, {dutyCycle2}");
        }

        public void SetSpeed(sbyte dutyCycle1, sbyte dutyCycle2)
        {
            if (!_connection.WriteValue(CreateMessageConst((byte)dutyCycle1, (byte)dutyCycle2)))
            {
                LoggerHelper.Instance.Debug("Motor::SetSpeedTimed - failed to set speed");
                throw new Exception("Failed to set speed");
            }

            LoggerHelper.Instance.Debug($"Motor::SetSpeed speed = {dutyCycle1}, {dutyCycle2}");
        }

        public async void SetSpeedAsync(sbyte dutyCycle1, sbyte dutyCycle2)
        {
            await _connection.WriteValueAsync(CreateMessageConst((byte)dutyCycle1, (byte)dutyCycle2));

            LoggerHelper.Instance.Debug($"Motor::SetSpeedAsync speed = {dutyCycle1}, {dutyCycle2}");
        }

        //dutyCycle - is value for motor cycle. Not known if it will be used
        private byte[] CreateMessageConst(byte dutyCycle1, byte dutyCycle2)
        {
            byte[] message;
            //message to be sent to motor
            if (dutyCycle2 == 0)
            {
                message = (byte[])ConnectionConstants.CMD_MOTOR_CONSTANT_SINGLE.Clone();
            }
            else
            {
                message = (byte[])ConnectionConstants.CMD_MOTOR_CONSTANT_GROUP.Clone();
                message[7] = dutyCycle2;
            }
            message[3] = (byte)_port;
            message[6] = dutyCycle1;

            LoggerHelper.Instance.Debug($"Motor::CreateMessageConst {BitConverter.ToString(message)} created");

            return message;
        }

        //dutyCycle - is value for motor cycle. Not known if it will be used
        private byte[] CreateMessageTimed(ushort milliseconds, byte dutyCycle1, byte dutyCycle2)
        {
            byte[] message;
            //message to be sent to motor
            if (dutyCycle2 == 0)
            {
                message = (byte[])ConnectionConstants.CMD_MOTOR_TIMED_SINGLE.Clone();
            }
            else
            {
                message = (byte[])ConnectionConstants.CMD_MOTOR_TIMED_GROUP.Clone();
                message[9] = dutyCycle2;
            }
            message[3] = (byte) _port;

            //convert seconds to little endian (2 bytes)
            byte[] bytes = BitConverter.GetBytes(milliseconds);
            Array.Reverse(bytes);
            Array.Copy(bytes, 0, message, 6, 2);

            message[8] = dutyCycle1; 

            LoggerHelper.Instance.Debug($"Motor::CreateMessageTimed {BitConverter.ToString(message)} created");

            return message;
        }

        //dutyCycle - is value for motor cycle. Not known if it will be used
        private byte[] CreateMessageAngled(uint angle, byte dutyCycle1, byte dutyCycle2)
        {
            byte[] message;
            //message to be sent to motor
            if (dutyCycle2 == 0)
            {
                message = (byte[])ConnectionConstants.CMD_MOTOR_ANGLED_SINGLE.Clone();
            }
            else
            {
                message = (byte[])ConnectionConstants.CMD_MOTOR_ANGLED_GROUP.Clone();
                message[11] = dutyCycle2;
            }
            message[3] = (byte)_port;

            //convert angle to little endian (4 bytes)
            byte[] bytes = BitConverter.GetBytes(angle);
            Array.Reverse(bytes);
            Array.Copy(bytes, 0, message, 6, 4);

            message[10] = dutyCycle1;

            LoggerHelper.Instance.Debug($"Motor::CreateMessageAngled {BitConverter.ToString(message)} created");

            return message;
        }

        public event EventHandler OnActionStart;
        public event EventHandler OnActionFinished;
    }
}
