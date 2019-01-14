using System;
using Moq;
using NUnit.Framework;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

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
            Mock<GattCharacteristic> fake = new Mock<GattCharacteristic>();

            Button button = new Button(fake.Object);
            _pressed = false;

            button.OnButtonPressed += ButtonEvent_Pressed;
            button.FireEvent(ButtonStatus.BUTTON_PRESSED);
            Assert.True(_pressed);
        }

        [Test]
        public void TestButtonReleased()
        {
            Mock<GattCharacteristic> fake = new Mock<GattCharacteristic>();

            Button button = new Button(fake.Object);
            button.OnButtonReleased += ButtonEvent_Released;
            _released = false;
            button.FireEvent(ButtonStatus.BUTTON_RELEASED);
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
