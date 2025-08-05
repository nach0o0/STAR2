using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemoveRoleFromUser
{
    public record RemoveRoleFromUserCommand(
        Guid UserId,
        Guid RoleId,
        string Scope
    ) : ICommand;
}
