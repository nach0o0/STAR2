using MediatR;
using Planning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Domain.Events.PlanningEntries
{
    public record PlanningEntryUpdatedEvent(PlanningEntry PlanningEntry) : INotification;
}
