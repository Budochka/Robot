using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoBOOST.Constants;
using LegoBOOST.Helpers;
using LegoBOOST.Interfaces;

namespace LegoBOOST.Classes
{
    public class Button : IButton
    {
        internal Button(GattCharacteristic characteristic)
        {
            LoggerHelper.Instance.Debug("Button constructor called");

            AsyncHelpers.RunSync<GattCommunicationStatus>(() => characteristic.WriteValueAsync(ConnectionConstants.CMD_SUBSCRIBE_BUTTON.AsBuffer()).AsTask());
        }

        public event EventHandler OnButtonPressed;
        public event EventHandler OnButtonReleased;

        internal void FireEvent(ButtonStatus status)
        {
            LoggerHelper.Instance.Debug($"Button event called, status {status}");

            switch (status)
            {
                case ButtonStatus.BUTTON_RELEASED:
                {
                    EventHandler handler = OnButtonReleased;
                    handler?.Invoke(this, EventArgs.Empty);
                    break;
                }

                case ButtonStatus.BUTTON_PRESSED:
                {
                    EventHandler handler = OnButtonPressed;
                    handler?.Invoke(this, EventArgs.Empty);
                    break;
                }
            }
        }
    }
}