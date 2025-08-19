using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.DeletePlanningEntry
{
    public record DeletePlanningEntryCommand(Guid PlanningEntryId) : ICommand;
}
