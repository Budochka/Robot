using System;

namespace LegoBOOSTNet.Interfaces
{
    public class NotificationEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}