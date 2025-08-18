using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UnassignWorkModelFromEmployee
{
    public record UnassignWorkModelFromEmployeeCommand(
        Guid EmployeeWorkModelId,
        DateTime EndDate) : ICommand;
}
