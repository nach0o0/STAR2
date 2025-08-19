using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByGroup
{
    public record GetPlanningEntriesByGroupQueryResult(
        Guid Id,
        Guid EmployeeId,
        string EmployeeName,
        Guid CostObjectId,
        string CostObjectName,
        decimal PlannedHours,
        DateTime PlanningPeriodStart,
        DateTime PlanningPeriodEnd,
        string? Notes
    );
}
