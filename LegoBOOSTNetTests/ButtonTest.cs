using System;
using NUnit.Framework;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Telerik.JustMock;

namespace LegoBOOSTNetTests
{
    [TestFixture]
    public class ButtonTest
    {
        private bool _pressed;
        private bool _released;

        [Test]
        public void TestButtonPressed()
        {
            _pressed = false;
            var gc = Mock.Create<GattCharacteristic>(Constructor.NotMocked);
/*            Button button = new Button(gc);

            button.OnButtonPressed += ButtonEvent_Pressed;
            button.FireEvent(ButtonStatus.BUTTON_PRESSED);
*/
            Assert.True(_pressed);
        }

        [Test]
        public void TestButtonReleased()
        {
            _released = false;

            /*
            var gc = Mock.Create<GattCharacteristic>();
            Button button = new Button(gc);
            button.OnButtonReleased += ButtonEvent_Released;
            button.FireEvent(ButtonStatus.BUTTON_RELEASED);
 */
            Assert.True(_released);
        }

        private void ButtonEvent_Pressed(object sender, EventArgs e)
        {
            _pressed = true;
        }

        private void ButtonEvent_Released(object sender, EventArgs e)
        {
            _released = true;
        }
    }
}
