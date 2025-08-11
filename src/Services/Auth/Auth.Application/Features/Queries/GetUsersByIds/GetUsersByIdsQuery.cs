using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUsersByIds
{
    public record GetUsersByIdsQuery(List<Guid> UserIds)
        : IRequest<List<GetUsersByIdsQueryResult>>;
}
