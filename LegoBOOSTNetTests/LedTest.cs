using System;
using System.Threading;
using System.Threading.Tasks;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using LegoBOOSTNet.Helpers;
using LegoBOOSTNet.Interfaces;
using Telerik.JustMock;
using NUnit.Framework;

namespace LegoBOOSTNetTests
{
    [TestFixture]
    public class LEDTest
    {
        private bool _changed;

        //check that the message sent is the one we expected
        [Test]
        public void TestChangeColor()
        {
            var connection = Mock.Create<IConnection>();

            //message that we want to see. Last byte is the color index (see Color enum)
            var data = new byte[8] { 0x08, 0x00, 0x81, 0x32, 0x11, 0x51, 0x00, 0x01};

            //setting mock
            Mock.Arrange(() => connection.WriteValue(data)).Returns((byte[] b) => true).MustBeCalled();

            var led = new LED(connection, Ports.PORT_LED);
            led.SetColor(Color.Pink);

            Mock.Assert(connection);
        }

        //check that the message sent fires no exception and firing event works fine
        [Test]
        public async Task TestChangeColorAsync()
        {
            _changed= false;
            var connection = Mock.Create<IConnection>();

            //setting mock
            Mock.Arrange(() => connection.WriteValue(Arg.IsAny<byte[]>())).Returns((byte[] b) => true);

            var led = new LED(connection, Ports.PORT_LED);
            led.OnColorChanged += ColorChanges;
            await led.SetColorAsync(Color.Red);

            led.FireOnColorChanged();
            Assert.True(_changed);
        }

        private void ColorChanges(object sender, EventArgs e)
        {
            _changed = true;
        }
    }
}