// ReSharper disable InconsistentNaming

namespace LegoBOOSTNet.Constants
{
    static class ConnectionConstants
    {
        public const byte PACKET_VER = 0x01;

        public const ulong AdreessLEGO = 0x1653A7DE77;
        public const string ServiceUUID = "00001623-1212-efde-1623-785feabcd123";
        public const string CharacteristicUUID = "00001624-1212-efde-1623-785feabcd123";

        //messages 
        public static readonly byte[] TRAILER = new byte[] { 0x64, 0x7f, 0x03 };
        
        //need to set 4th byte to exact port for subscription, 5th byte is sensor mode
        public static readonly byte[] CMD_SUBSCRIBE_DISTANCE_COLOR = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01 };

        public static readonly byte[] CMD_SUBSCRIBE_ANGLE = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01 };
        public static readonly byte[] CMD_SUBSCRIBE_SPEED = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x01 };

        public static readonly byte[] CMD_SUBSCRIBE_BUTTON = new byte[] { 0x05, 0x00, 0x01, 0x02, 0x02 };

        public static readonly byte[] EVENT_BUTTON_PRESSED = new byte[] { 0x06, 0x00, 0x01, 0x02, 0x06, 0x01 };
        public static readonly byte[] EVENT_BUTTON_RELEASED = new byte[] { 0x06, 0x00, 0x01, 0x02, 0x06, 0x00 };

        //5th byte is sensor mode. Need to be set to one of SensorMode values before sent, 6th and 7th - notifications granularity, 11th  - 01 subscribe, 00 - unsubscribe
        public static readonly byte[] CMD_SUBSCRIBE_TILT_SENSOR = new byte[] { 0x0a, 0x00, 0x41, 0x3a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
    }
}