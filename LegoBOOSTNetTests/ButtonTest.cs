using System;
using Pose;
using NUnit.Framework;
using LegoBOOSTNet.Classes;
using LegoBOOSTNet.Constants;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Is = Pose.Is;

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
            Shim classShim = Shim.Replace(() => Is.A<GattCharacteristic>().WriteValueAsync(Is.A<IBuffer>())).With(
                delegate(GattCharacteristic @this) { Console.WriteLine("doing someting else"); } );
            PoseContext.Isolate(() =>
            {
                GattCharacteristic gc = new GattCharacteristic();
                Button button = new Button(fake.Object);
                _pressed = false;

                button.OnButtonPressed += ButtonEvent_Pressed;
                button.FireEvent(ButtonStatus.BUTTON_PRESSED);
            });
            Assert.True(_pressed);
        }

        [Test]
        public void TestButtonReleased()
        {
            Shim classShim = Shim.Replace(() => Pose.Is.A<GattCharacteristic>().WriteValueAsync(Pose.Is.A<IBuffer>())).With(
                delegate (GattCharacteristic @this) { Console.WriteLine("doing someting else"); });

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
