using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetHourlyRatesByOrganization
{
    public record GetHourlyRatesByOrganizationQueryResult(
        Guid Id,
        string Name,
        decimal Rate,
        DateTime ValidFrom,
        DateTime? ValidTo,
        string? Description
    );
}
