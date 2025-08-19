using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByEmployee
{
    public record GetPlanningEntriesByEmployeeQueryResult(
        Guid Id,
        Guid CostObjectId,
        string CostObjectName,
        decimal PlannedHours,
        DateTime PlanningPeriodStart,
        DateTime PlanningPeriodEnd,
        string? Notes
    );
}
