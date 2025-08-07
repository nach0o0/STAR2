using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Messages
{
    public record UserLoggedInMessage(CurrentUser User);
}
