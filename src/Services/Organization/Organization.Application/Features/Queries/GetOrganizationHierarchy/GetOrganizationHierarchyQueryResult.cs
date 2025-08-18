using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationHierarchy
{
    public record GetOrganizationHierarchyQueryResult(
        Guid Id,
        string Name,
        string Abbreviation,
        List<GetOrganizationHierarchyQueryResult> Children
    );
}
