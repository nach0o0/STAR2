using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.CreateTimeEntry
{
    public record CreateTimeEntryCommandResult(
        Guid TimeEntryId,
        Guid? AccessKey
    );
}
