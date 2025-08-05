using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AddPermissionToRole
{
    public record AddPermissionToRoleCommand(
        Guid RoleId,
        string PermissionId
    ) : ICommand;
}
