using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Messages
{
    public enum StatusMessageType { Success, Error }

    public record StatusUpdateMessage(string Message, StatusMessageType MessageType);
}
