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
        //need to set 4th byte to exact port for subscription, 5th byte is sensor mode, last byte  - 01 subscribe, 00 - unsubscribe
        public static readonly byte[] CMD_SUBSCRIBE_DISTANCE_COLOR = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01 };

        public static readonly byte[] CMD_SUBSCRIBE_ANGLE = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01 };
        public static readonly byte[] CMD_SUBSCRIBE_SPEED = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x01 };

        public static readonly byte[] CMD_SUBSCRIBE_BUTTON = new byte[] { 0x05, 0x00, 0x01, 0x02, 0x02 };

        public static readonly byte[] EVENT_BUTTON_PRESSED = new byte[] { 0x06, 0x00, 0x01, 0x02, 0x06, 0x01 };
        public static readonly byte[] EVENT_BUTTON_RELEASED = new byte[] { 0x06, 0x00, 0x01, 0x02, 0x06, 0x00 };

        //5th byte is sensor mode. Need to be set to one of SensorMode values before sent, 6th and 7th - notifications granularity, 9th  - 01 subscribe, 00 - unsubscribe
        public static readonly byte[] CMD_SUBSCRIBE_TILT_SENSOR = new byte[] { 0x0a, 0x00, 0x41, 0x3a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

        //motor messages
        // 4th byte - port
        // 6th byte - value type
        // for timed values:
        // 7+8 - motor run time in milliseconds in little endian (UInt16)
        // 9 - duty cycle 0..127, including turn direction (+/-)
        // for angled values:
        // 7...10 - angle(UInt32 Little Endian)
        // 11 - dutycycle 0..127, including turn direction(+/-)
        // last 3 bytes is trailer
        public static readonly byte[] CMD_MOTOR_CONSTANT_SINGLE = new byte [] { 0x0a, 0x00, 0x81, 0x00, 0x11, 0x01, 0x00, 0x64, 0x7f, 0x03 };
        public static readonly byte[] CMD_MOTOR_CONSTANT_GROUP = new byte [] { 0x0b, 0x00, 0x81, 0x00, 0x11, 0x02, 0x00, 0x00, 0x64, 0x7f, 0x03 };
        public static readonly byte[] CMD_MOTOR_TIMED_SINGLE = new byte [] { 0x0c, 0x00, 0x81, 0x00, 0x11, 0x09, 0x00, 0x00, 0x00, 0x64, 0x7f, 0x03 };
        public static readonly byte[] CMD_MOTOR_TIMED_GROUP = new byte [] { 0x0d, 0x00, 0x81, 0x00, 0x11, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x64, 0x7f, 0x03 };
        public static readonly byte[] CMD_MOTOR_ANGLED_SINGLE = new byte [] { 0x0e, 0x00, 0x81, 0x00, 0x11, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x7f, 0x03 };
        public static readonly byte[] CMD_MOTOR_ANGLED_GROUP = new byte [] { 0x0f, 0x00, 0x81, 0x00, 0x11, 0x0c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x7f, 0x03 };

    }
}