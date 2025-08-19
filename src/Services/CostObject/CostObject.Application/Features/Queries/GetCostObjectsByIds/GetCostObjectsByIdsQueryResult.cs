using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByIds
{
    public record GetCostObjectsByIdsQueryResult(
        Guid Id,
        string Name,
        Guid EmployeeGroupId
    );
}
