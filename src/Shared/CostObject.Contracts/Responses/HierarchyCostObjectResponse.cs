using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CostObject.Contracts.Responses
{
    public record HierarchyCostObjectResponse(
        Guid Id,
        string Name,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        ApprovalStatusDto ApprovalStatus,
        List<HierarchyCostObjectResponse> Children
    );
}
