using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.CreateRole
{
    public record CreateRoleCommand(
        string Name,
        string Description,
        string? Scope
    ) : ICommand<Guid>;
}
