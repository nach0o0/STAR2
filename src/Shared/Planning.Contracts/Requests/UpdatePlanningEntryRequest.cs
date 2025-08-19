using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Contracts.Requests
{
    public record UpdatePlanningEntryRequest(
        decimal PlannedHours,
        string? Notes
    );
}
