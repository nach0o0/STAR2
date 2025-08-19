using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetHierarchyDefinitionsByGroup
{
    public record GetHierarchyDefinitionsByGroupQueryResult(
        Guid Id,
        string Name,
        Guid? RequiredBookingLevelId
    );
}
