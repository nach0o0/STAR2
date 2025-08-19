using CostObject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectRequestsByGroup
{
    public record GetCostObjectRequestsByGroupQueryResult(
        Guid RequestId,
        Guid CostObjectId,
        string CostObjectName,
        Guid RequesterEmployeeId,
        ApprovalStatus Status,
        DateTime CreatedAt
    );
}
