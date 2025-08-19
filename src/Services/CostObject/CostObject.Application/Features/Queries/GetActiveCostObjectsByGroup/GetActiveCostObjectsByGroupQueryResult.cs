using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetActiveCostObjectsByGroup
{
    public record GetActiveCostObjectsByGroupQueryResult(
        Guid Id,
        string Name,
        Guid? ParentCostObjectId,
        int Depth
    );
}
