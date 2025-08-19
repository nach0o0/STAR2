using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CostObject.Contracts.Responses
{
    public record CostObjectRequestResponse(
        Guid RequestId,
        Guid CostObjectId,
        string CostObjectName,
        Guid RequesterEmployeeId,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        ApprovalStatusDto Status,
        DateTime CreatedAt
    );
}
