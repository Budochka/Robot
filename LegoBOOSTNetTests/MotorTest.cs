using System;
using System.Threading.Tasks;
using NUnit.Framework;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Interfaces;
using Telerik.JustMock;

namespace LegoBOOSTNetTests
{
    [TestFixture]
    public class MotorTest
    {
        private readonly byte[] _trailer = { 0x64, 0x7f, 0x03 };

        [Test]
        public void TestMotorSpin()
        {
            var connection = Mock.Create<IConnection>();

            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);

            var motor = new Motor(connection, Ports.PORT_A);
            ushort time = 1000;
            sbyte dutyCycle = -100;

            //message we are expecting to receive
            byte[] data = {12, 0, 0x81, (byte)Ports.PORT_A, 0x11, 0x09, 0, 0, 0, 0, 0, 0 };
            byte[] bytes = BitConverter.GetBytes(time);
            Array.Reverse(bytes);
            data[6] = bytes[0];
            data[7] = bytes[1];
            data[8] = (byte) dutyCycle;
            Array.Copy(_trailer, 0, data, 9, _trailer.Length);

            Mock.Arrange(() => connection.WriteValue(data)).Returns((byte[] b) => true).MustBeCalled();
            motor.SetSpeedTimed(time, dutyCycle, 0);

            Mock.Assert(connection);
        }

        [Test]
        public void TestMotorSpinAsync()
        {
            var connection = Mock.Create<IConnection>();

            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);

            var motor = new Motor(connection, Ports.PORT_A);
            ushort time = 1000;
            sbyte dutyCycle = -100;

            //message we are expecting to receive
            byte[] data = new byte[12] { 12, 0, 0x81, (byte)Ports.PORT_A, 0x11, 0x09, 0, 0, 0, 0, 0, 0 };
            byte[] bytes = BitConverter.GetBytes(time);
            Array.Reverse(bytes);
            data[6] = bytes[0];
            data[7] = bytes[1];
            data[8] = (byte)dutyCycle;
            Array.Copy(_trailer, 0, data, 9, _trailer.Length);

            Mock.Arrange(() => connection.WriteValueAsync(data)).MustBeCalled();
            motor.SetSpeedTimedAsync(time, dutyCycle, 0);

            Mock.Assert(connection);
        }
    }
}