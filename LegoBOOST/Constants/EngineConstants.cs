﻿// ReSharper disable InconsistentNaming
namespace LegoBOOST
{
    namespace Constants
    {
        public enum OperationResult
        {
            Success,
            Failure
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
            PORT_AMPERAGE = 0x3B,
            PORT_VOLTAGE = 0x3C
        }

        public enum PacketType
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

        public enum DeviceTypes
        {
            DEV_VOLTAGE = 0x14,
            DEV_AMPERAGE = 0x15,
            DEV_LED = 0x17,
            DEV_DCS = 0x25,
            DEV_IMOTOR = 0x26,
            DEV_MOTOR = 0x27,
            DEV_TILT_SENSOR = 0x28
        }

        public enum Statuses
        {
            STATUS_STARTED = 0x01,
            STATUS_CONFLICT = 0x05,
            STATUS_FINISHED = 0x0a,
            STATUS_INPROGRESS = 0x0c,  //FIXME: not sure about description
            STATUS_INTERRUPTED = 0x0e  //FIXME:  not sure about description
        }

        public enum DeviceInfo
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

        public enum ActionInfo
        {
            INFO_ACTION_SUBSCRIBE = 0x02,
            INFO_ACTION_UNSUBSCRIBE = 0x03,
            INFO_ACTION_GET = 0x05
        }

        public enum Orientation
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
            Black = 0,
            Pink,
            Purple,
            Blue,
            LightBlue,
            Cyan,
            Green,
            Yellow,
            Orange,
            Red,
            White,
            None = 0xFF
        }
    }
}