using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Queries.ListMySessions
{
    public record ListMySessionsQueryResult(
        Guid SessionId,
        DateTime CreatedAt,
        string ClientInfo
    );
}
