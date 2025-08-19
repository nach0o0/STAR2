using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CostObject.Contracts.Responses
{
    public record TopLevelCostObjectResponse(
        Guid Id,
        string Name,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        ApprovalStatusDto ApprovalStatus
    );
}
