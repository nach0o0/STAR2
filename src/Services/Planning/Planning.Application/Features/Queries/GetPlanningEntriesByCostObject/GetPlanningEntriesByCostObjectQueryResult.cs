using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByCostObject
{
    public record GetPlanningEntriesByCostObjectQueryResult(
        Guid Id,
        Guid EmployeeId,
        string EmployeeName,
        decimal PlannedHours,
        DateTime PlanningPeriodStart,
        DateTime PlanningPeriodEnd,
        string? Notes
    );
}
