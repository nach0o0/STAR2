using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Contracts.Requests
{
    public record UpdateCostObjectRequest(
        string? Name,
        Guid? ParentCostObjectId,
        Guid? HierarchyLevelId,
        Guid? LabelId
    );
}
