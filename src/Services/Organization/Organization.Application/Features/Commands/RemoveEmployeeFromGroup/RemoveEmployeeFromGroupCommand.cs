using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RemoveEmployeeFromGroup
{
    public record RemoveEmployeeFromGroupCommand(
        Guid EmployeeId,
        Guid EmployeeGroupId
    ) : ICommand;
}
