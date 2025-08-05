using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Contracts.Responses
{
    public record ActiveSessionResponse(
        Guid SessionId,
        DateTime CreatedAt,
        string ClientInfo
    );
}
