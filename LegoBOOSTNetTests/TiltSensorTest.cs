using System;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Interfaces;
using NUnit.Framework;
using Telerik.JustMock;

namespace LegoBOOSTNetTests
{
    [TestFixture]
    public class TiltSensorTest
    {
        private TiltSensorEventArgs _expectedResult;
        private bool _success;

        //test setting notification mode
        [Test]
        public void TestModeChanged()
        {
            var connection = Mock.Create<IConnection>();

            //message that we want to see. Last byte is subscribe/unsubscribe flag
            var dataSubscribe = (byte[])ConnectionConstants.CMD_SUBSCRIBE_TILT_SENSOR.Clone();
            var dataUnsubscribe = (byte[])ConnectionConstants.CMD_SUBSCRIBE_TILT_SENSOR.Clone();

            //set the mode
            dataSubscribe[5] = dataUnsubscribe[5] = (byte)SensorMode.SENSOR_3_AXIS_PRECISE;
            //01 - subscribe, 00 - unsubscribe
            dataSubscribe[9] = 0x01;
            dataUnsubscribe[9] = 0x00;

            //setting mock
            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);
            Mock.Arrange(() => connection.WriteValue(dataSubscribe)).Returns((byte[] b) => true).MustBeCalled();
            Mock.Arrange(() => connection.WriteValue(dataUnsubscribe)).Returns((byte[] b) => true).MustBeCalled();

            var sensor = new TiltSensor(connection, Ports.PORT_TILT_SENSOR);
            //setting the mode
            sensor.SetNotificationMode(SensorMode.SENSOR_3_AXIS_PRECISE);

            //setting another mode - unsubscribe should be called
            sensor.SetNotificationMode(SensorMode.SENSOR_BUMP_DETECT);

            Mock.Assert(sensor);
        }

        //check correct fields order in notification events for COLOR_DISTANCE_FLOAT mode
        [Test]
        public void TestNotificationReceived()
        {
            var connection = Mock.Create<IConnection>();
            _success = false;

            //setting mock
            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);

            var sensor = new TiltSensor(connection, Ports.PORT_D);
            sensor.OnChange += Changes;

            _expectedResult = new TiltSensorEventArgs
            {
                Roll = 1,
                Pitch = 1,
                Yaw = 1
            };

            var data = new byte[] { 0, 0, 0, 0, (byte)_expectedResult.Roll, (byte)_expectedResult.Pitch, (byte)_expectedResult.Yaw, 0 };

            sensor.SetNotificationMode(SensorMode.SENSOR_3_AXIS_PRECISE);
            sensor.FireEvent(data);

            Assert.True(_success);
        }

        private void Changes(object sender, TiltSensorEventArgs e)
        {
            _success = (e.Pitch == _expectedResult.Pitch) &&
                       (e.Roll == _expectedResult.Roll) &&
                       (e.Yaw == _expectedResult.Yaw);
        }
    }
}