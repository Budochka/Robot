using System;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Interfaces;
using NUnit.Framework;
using Telerik.JustMock;

namespace LegoBOOSTNetTests
{
    [TestFixture]
    public class DistanceColorSensorTest
    {
        private DistanceColorSensorEventArgs _expectedResult;
        private bool _success;

        [Test]
        public void TestModeChanged()
        {
            var connection = Mock.Create<IConnection>();

            //message that we want to see. Last byte is the color index (see Color enum)
            var dataSubscribe = (byte[])ConnectionConstants.CMD_SUBSCRIBE_DISTANCE_COLOR.Clone();
            var dataUnsubscribe = (byte[])ConnectionConstants.CMD_SUBSCRIBE_DISTANCE_COLOR.Clone();

            //set the port
            dataSubscribe[3] = dataUnsubscribe[3] = (byte)Ports.PORT_D;
            //set the mode
            dataSubscribe[4] = dataUnsubscribe[4] = (byte)DistanceColorSensorMods.COLOR_DISTANCE_FLOAT;
            //01 - subscribe, 00 - unsubscribe
            dataSubscribe[9] = 0x01;
            dataUnsubscribe[9] = 0x00;


            //setting mock
            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);
            Mock.Arrange(() => connection.WriteValue(dataSubscribe)).Returns((byte[] b) => true).MustBeCalled();
            Mock.Arrange(() => connection.WriteValue(dataUnsubscribe)).Returns((byte[] b) => true).MustBeCalled();

            var sensor = new DistanceColorSensor(connection, Ports.PORT_D);
            //setting the mode
            sensor.SetMode(DistanceColorSensorMods.COLOR_DISTANCE_FLOAT);

            //setting another mode - unsubscribe should be called
            sensor.SetMode(DistanceColorSensorMods.COLOR_ONLY);

            Mock.Assert(sensor);
        }

        //check correct fields order in notification events
        [Test]
        public void TestNotificationReceived()
        {
            var connection = Mock.Create<IConnection>();

            //setting mock
            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);

            var sensor = new DistanceColorSensor(connection, Ports.PORT_D);
            sensor.OnChange += Changes;

            _expectedResult = new DistanceColorSensorEventArgs
            {
                ColorDetected = Color.Red,
                Distance = 2,
                Partial = 0.5
            };

            var data = new byte[] {0, 0, 0, 0, (byte)_expectedResult.ColorDetected, (byte)_expectedResult.Distance, 0, (byte)(1.0/_expectedResult.Partial) };

            sensor.SetMode(DistanceColorSensorMods.COLOR_DISTANCE_FLOAT);
            sensor.FireEvent(data);

            Assert.True(_success);
        }

        private void Changes(object sender, DistanceColorSensorEventArgs e)
        {
            _success = (e.Distance ==_expectedResult.Distance) &&
                       (e.ColorDetected == _expectedResult.ColorDetected) &&
                       (Math.Abs(e.Partial - _expectedResult.Partial) < 0.01);
        }
    }
}