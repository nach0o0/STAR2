using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetTopLevelOrganizations
{
    public record GetTopLevelOrganizationsQueryResult(
        Guid Id,
        string Name,
        string Abbreviation
    );
}
