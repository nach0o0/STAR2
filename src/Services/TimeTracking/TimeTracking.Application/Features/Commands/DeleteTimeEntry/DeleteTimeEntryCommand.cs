using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.DeleteTimeEntry
{
    public record DeleteTimeEntryCommand(
        Guid TimeEntryId,
        Guid? AccessKey
    ) : ICommand;
}
