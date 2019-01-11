namespace LegoBOOSTNet.Constants
{
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
}