using MediatR;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RegisterPermissions
{
    public class RegisterPermissionsCommandHandler : IRequestHandler<RegisterPermissionsCommand>
    {
        private readonly IPermissionRepository _permissionRepository;

        public RegisterPermissionsCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task Handle(RegisterPermissionsCommand command, CancellationToken cancellationToken)
        {
            // Wandelt die Tupel-Liste in eine Liste von Permission-Entitäten um.
            var permissions = command.Permissions
                .Select(p => new Domain.Entities.Permission(p.Id, p.Description, p.PermittedScopeTypes))
                .ToList();

            // Verwendet das Repository, um die neuen Berechtigungen idempotent hinzuzufügen.
            await _permissionRepository.AddRangeAsync(permissions, cancellationToken);
        }
    }
}
