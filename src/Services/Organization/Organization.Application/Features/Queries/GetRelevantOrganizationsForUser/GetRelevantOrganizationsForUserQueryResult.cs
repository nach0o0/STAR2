using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetRelevantOrganizationsForUser
{
    public record GetRelevantOrganizationsForUserQueryResult(
        Guid Id,
        string Name,
        bool IsPrimary
    );
}
