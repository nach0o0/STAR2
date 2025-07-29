using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Requests
{
    public record CreateHourlyRateRequest(
        string Name,
        decimal Rate,
        DateTime ValidFrom,
        Guid OrganizationId,
        DateTime? ValidTo,
        string? Description
    );
}
