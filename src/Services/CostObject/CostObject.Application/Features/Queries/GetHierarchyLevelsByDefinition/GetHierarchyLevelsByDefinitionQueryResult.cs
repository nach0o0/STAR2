using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetHierarchyLevelsByDefinition
{
    public record GetHierarchyLevelsByDefinitionQueryResult(
        Guid Id,
        string Name,
        int Depth
    );
}
