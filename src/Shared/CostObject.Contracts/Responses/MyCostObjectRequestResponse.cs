using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CostObject.Contracts.Responses
{
    public record MyCostObjectRequestResponse(
        Guid RequestId,
        Guid CostObjectId,
        string CostObjectName,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        ApprovalStatusDto Status,
        DateTime CreatedAt,
        string? RejectionReason
    );
}
