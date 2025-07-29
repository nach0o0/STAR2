using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RegisterPermissions
{
    public record RegisterPermissionsCommand(
        IEnumerable<(string Id, string Description)> Permissions
    ) : IRequest;
}
