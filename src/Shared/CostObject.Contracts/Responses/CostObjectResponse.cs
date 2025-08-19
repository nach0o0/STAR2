using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CostObject.Contracts.Responses
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApprovalStatusDto { Pending, Approved, Rejected }

    public record CostObjectResponse(
        Guid Id,
        string Name,
        Guid EmployeeGroupId,
        Guid? ParentCostObjectId,
        Guid HierarchyLevelId,
        Guid? LabelId,
        DateTime ValidFrom,
        DateTime? ValidTo,
        ApprovalStatusDto ApprovalStatus
    );
}
