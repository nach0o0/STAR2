using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.UpdateTimeEntry
{
    public record UpdateTimeEntryCommand(
        Guid TimeEntryId,
        DateTime? EntryDate,
        Guid? CostObjectId,
        decimal? Hours,
        string? Description,
        Guid? AccessKey
    ) : ICommand;
}
