using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.UpdateRole
{
    public record UpdateRoleCommand(
        Guid RoleId,
        string? Name,
        string? Description
    ) : ICommand;
}
