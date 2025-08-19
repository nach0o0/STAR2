using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Contracts.Responses
{
    public record PlanningEntriesByEmployeeResponse(
        Guid Id,
        Guid CostObjectId,
        string CostObjectName,
        decimal PlannedHours,
        DateTime PlanningPeriodStart,
        DateTime PlanningPeriodEnd,
        string? Notes
    );
}
