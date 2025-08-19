using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Contracts.Requests
{
    public record CreateCostObjectRequestRequest(
        string Name,
        Guid EmployeeGroupId,
        Guid? ParentCostObjectId,
        Guid HierarchyLevelId,
        Guid? LabelId,
        DateTime ValidFrom
    );
}
