// ReSharper disable InconsistentNaming

using System;

namespace LegoBOOST.Constants
{
    static class ConnectionConstants
    {
        public const byte PACKET_VER = 0x01;

        public const ulong AdreessLEGO = 0x1653A7DE77;
        public const string ServiceUUID = "00001623-1212-efde-1623-785feabcd123";
        public const string CharacteristicUUID = "00001624-1212-efde-1623-785feabcd123";

        //messages 
        public static readonly byte[] TRAILER = new byte[] { 0x64, 0x7f, 0x03 };
        
        //need to set 4th byte to exact port for subscription
        public static readonly byte[] CMD_SUBSCRIBE_DISTANCE_COLOR = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x08, 0x01, 0x00, 0x00, 0x00, 0x01 };

        public static readonly byte[] CMD_SUBSCRIBE_ANGLE = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01 };
        public static readonly byte[] CMD_SUBSCRIBE_SPEED = new byte[] { 0x0a, 0x00, 0x41, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x01 };

        public static readonly byte[] CMD_SUBSCRIBE_BUTTON = new byte[] { 0x05, 0x00, 0x01, 0x02, 0x02 };

        public static readonly byte[] EVENT_BUTTON_PRESSED = new byte[] { 0x06, 0x00, 0x01, 0x02, 0x06, 0x01 };
        public static readonly byte[] EVENT_BUTTON_RELEASED = new byte[] { 0x06, 0x00, 0x01, 0x02, 0x06, 0x00 };
    }

    public enum Ports
    {
        PORT_C = 0x01,
        PORT_D = 0x02,
        PORT_LED = 0x32,
        PORT_A = 0x37,
        PORT_B = 0x38,
        PORT_AB = 0x39,
        PORT_TILT_SENSOR = 0x3A,
        PORT_AMPERAGE = 0x3B, //never used
        PORT_VOLTAGE = 0x3C //never used
    }

    internal enum PacketType
    {
        MSG_DEVICE_INFO = 0x01,
        MSG_DEVICE_SHUTDOWN = 0x02,  //sent when hub shuts down by button hold
        MSG_PING_RESPONSE = 0x03,
        MSG_PORT_INFO = 0x04,
        MSG_PORT_CMD_ERROR = 0x05,
        MSG_SET_PORT_VAL = 0x81,
        MSG_PORT_STATUS = 0x82,
        MSG_SENSOR_SUBSCRIBE = 0x41,
        MSG_SENSOR_SOMETHING1 = 0x42,  //it is seen close to sensor subscribe commands. Subscription options? Initial value?
        MSG_SENSOR_DATA = 0x45,
        MSG_SENSOR_SUBSCRIBE_ACK = 0x47
    }

    internal enum DeviceTypes
    {
        DEV_VOLTAGE = 0x14,
        DEV_AMPERAGE = 0x15,
        DEV_LED = 0x17,
        DEV_DCS = 0x25,
        DEV_IMOTOR = 0x26,
        DEV_MOTOR = 0x27,
        DEV_TILT_SENSOR = 0x28
    }

    internal enum ActionStatuses
    {
        STATUS_STARTED = 0x01,
        STATUS_CONFLICT = 0x05,
        STATUS_FINISHED = 0x0a,
        STATUS_INPROGRESS = 0x0c,  //FIXME: not sure about description
        STATUS_INTERRUPTED = 0x0e  //FIXME:  not sure about description
    }

    internal enum ButtonStatus
    {
        BUTTON_RELEASED = 0x00,
        BUTTON_PRESSED = 0x01
    }

    internal enum DeviceInfo
    {
        INFO_DEVICE_NAME = 0x01,
        INFO_BUTTON_STATE = 0x02,
        INFO_FIRMWARE_VERSION = 0x03,
        INFO_SOME4 = 0x04,
        INFO_SOME5_JITTERING = 0x05,
        INFO_SOME6 = 0x06,
        INFO_SOME7 = 0x07,
        INFO_MANUFACTURER = 0x08,
        INFO_HW_VERSION = 0x09,
        INFO_SOME10 = 0x0a,
        INFO_SOME11 = 0x0b,
        INFO_SOME12 = 0x0c
    }

    internal enum ActionInfo
    {
        INFO_ACTION_SUBSCRIBE = 0x02,
        INFO_ACTION_UNSUBSCRIBE = 0x03,
        INFO_ACTION_GET = 0x05
    }

    internal enum Orientation
    {
        Back,
        Up,
        Down,
        Left,
        Right,
        Front
    }

    public enum Color
    {
        None = 0,
        Pink,
        Purple,
        Blue,
        LightBlue,
        Cyan,
        Green,
        Yellow,
        Orange,
        Red,
        White
    }

    public enum MotorGroup
    {
        CONSTANT_SINGLE = 0x01,
        CONSTANT_GROUP = 0x02,
        SOME_SINGLE = 0x07,
        SOME_GROUP = 0x08,
        TIMED_SINGLE = 0x09,
        TIMED_GROUP = 0x0A,
        ANGLED_SINGLE = 0x0B,
        ANGLED_GROUP = 0x0C
    }

    public enum MotorSensor
    {
        SENSOR_SOMETHING1 = 0x00,  
        SENSOR_SPEED = 0x01,
        SENSOR_ANGLE = 0x02
    }
}