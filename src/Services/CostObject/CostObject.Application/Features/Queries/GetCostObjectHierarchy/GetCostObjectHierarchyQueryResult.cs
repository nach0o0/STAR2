using CostObject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectHierarchy
{
    public record GetCostObjectHierarchyQueryResult(
        Guid Id,
        string Name,
        ApprovalStatus ApprovalStatus,
        List<GetCostObjectHierarchyQueryResult> Children
    );
}
