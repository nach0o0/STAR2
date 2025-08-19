using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByCostObject
{
    public record GetTimeEntriesByCostObjectQuery(
        Guid CostObjectId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<List<GetTimeEntriesByCostObjectQueryResult>>;
}
