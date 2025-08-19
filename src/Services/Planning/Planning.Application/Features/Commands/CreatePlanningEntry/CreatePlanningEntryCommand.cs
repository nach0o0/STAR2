using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.CreatePlanningEntry
{
    public record CreatePlanningEntryCommand(
        Guid EmployeeGroupId,
        Guid EmployeeId,
        Guid CostObjectId,
        decimal PlannedHours,
        DateTime PlanningPeriodStart,
        DateTime PlanningPeriodEnd,
        string? Notes
    ) : IRequest<Guid>;
}
