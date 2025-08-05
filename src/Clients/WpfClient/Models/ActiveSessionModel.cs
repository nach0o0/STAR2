using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Models
{
    public class ActiveSessionModel
    {
        public Guid SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ClientInfo { get; set; }
    }
}
