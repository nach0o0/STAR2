using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.AnonymizeTimeEntry
{
    public record AnonymizeTimeEntryCommand(Guid TimeEntryId) : IRequest<AnonymizeTimeEntryCommandResult>;
}
