using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.AssignHourlyRateToEmployee
{
    public record AssignHourlyRateToEmployeeCommand(
        Guid EmployeeId,
        Guid EmployeeGroupId,
        Guid? HourlyRateId
    ) : ICommand;
}
