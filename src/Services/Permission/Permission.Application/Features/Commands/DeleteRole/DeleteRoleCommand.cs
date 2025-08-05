using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.DeleteRole
{
    public record DeleteRoleCommand(Guid RoleId) : ICommand;
}
