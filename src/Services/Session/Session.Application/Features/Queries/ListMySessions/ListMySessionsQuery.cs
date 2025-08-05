using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Queries.ListMySessions
{
    public record ListMySessionsQuery : IRequest<List<ListMySessionsQueryResult>>;
}
