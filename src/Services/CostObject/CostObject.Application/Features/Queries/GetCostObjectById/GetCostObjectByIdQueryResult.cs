using CostObject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectById
{
    public record GetCostObjectByIdQueryResult(
        Guid Id,
        string Name,
        Guid EmployeeGroupId,
        Guid? ParentCostObjectId,
        Guid HierarchyLevelId,
        Guid? LabelId,
        DateTime ValidFrom,
        DateTime? ValidTo,
        ApprovalStatus ApprovalStatus
    );
}
