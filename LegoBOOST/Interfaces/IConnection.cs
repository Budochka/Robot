using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoBOOSTNet.Interfaces
{
    public interface IConnection
    {
        //low level interface
        bool WriteValue(byte[] data);
        Task<bool> WriteValueAsync(byte[] data);

        event EventHandler<NotificationEventArgs> OnChange;
    }
}
