using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Contracts.Responses
{
    public record ActiveCostObjectResponse(
        Guid Id,
        string Name,
        Guid? ParentCostObjectId,
        int Depth
    );
}
