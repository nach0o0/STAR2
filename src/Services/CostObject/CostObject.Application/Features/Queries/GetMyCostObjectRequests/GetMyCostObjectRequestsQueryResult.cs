using CostObject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetMyCostObjectRequests
{
    public record GetMyCostObjectRequestsQueryResult(
        Guid RequestId,
        Guid CostObjectId,
        string CostObjectName,
        ApprovalStatus Status,
        DateTime CreatedAt,
        string? RejectionReason
    );
}
