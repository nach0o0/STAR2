using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.AssignWorkModelToEmployee
{
    public record AssignWorkModelToEmployeeCommand(
        Guid EmployeeId,
        Guid WorkModelId,
        DateTime ValidFrom,
        DateTime? ValidTo) : ICommand<Guid>;
}
