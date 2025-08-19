using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByCostObject
{
    public record GetPlanningEntriesByCostObjectQuery(
        Guid CostObjectId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<List<GetPlanningEntriesByCostObjectQueryResult>>;
}
