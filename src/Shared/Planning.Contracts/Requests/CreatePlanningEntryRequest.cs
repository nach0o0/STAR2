using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Contracts.Requests
{
    public record CreatePlanningEntryRequest(
        Guid EmployeeGroupId,
        Guid EmployeeId,
        Guid CostObjectId,
        decimal PlannedHours,
        DateTime PlanningPeriodStart,
        DateTime PlanningPeriodEnd,
        string? Notes
    );
}
