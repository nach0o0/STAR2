using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Requests
{
    public record UpdateHourlyRateRequest(
        string? Name,
        decimal? Rate,
        DateTime? ValidFrom,
        DateTime? ValidTo,
        string? Description
    );
}
