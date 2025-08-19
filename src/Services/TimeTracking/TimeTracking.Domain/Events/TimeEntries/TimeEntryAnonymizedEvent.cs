using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Domain.Events.TimeEntries
{
    public record TimeEntryAnonymizedEvent(TimeEntry TimeEntry, Guid AccessKey) : INotification;
}
