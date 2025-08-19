using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetTopLevelCostObjectsByGroup
{
    public record GetTopLevelCostObjectsByGroupQuery(
        Guid EmployeeGroupId,
        bool ApprovedOnly = false,
        bool ActiveOnly = false
    ) : IRequest<List<GetTopLevelCostObjectsByGroupQueryResult>>;
}
