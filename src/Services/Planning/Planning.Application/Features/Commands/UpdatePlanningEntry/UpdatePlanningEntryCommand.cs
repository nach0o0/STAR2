using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.UpdatePlanningEntry
{
    public record UpdatePlanningEntryCommand(
        Guid PlanningEntryId,
        decimal PlannedHours,
        string? Notes
    ) : ICommand;
}
