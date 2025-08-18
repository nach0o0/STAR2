using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateEmployeeWorkModelAssignment
{
    public record UpdateEmployeeWorkModelAssignmentCommand(
        Guid AssignmentId,
        DateTime? ValidFrom,
        DateTime? ValidTo) : ICommand;
}
