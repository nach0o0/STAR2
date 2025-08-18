using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetHourlyRateById
{
    public record GetHourlyRateByIdQueryResult(
        Guid Id,
        string Name,
        decimal Rate,
        DateTime ValidFrom,
        DateTime? ValidTo,
        string? Description,
        Guid OrganizationId
    );
}
