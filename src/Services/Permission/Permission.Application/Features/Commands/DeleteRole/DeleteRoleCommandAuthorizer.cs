using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Authorization;
using Permission.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.DeleteRole
{
    public class DeleteRoleCommandAuthorizer : ICommandAuthorizer<DeleteRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleCommandAuthorizer(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task AuthorizeAsync(DeleteRoleCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            var requiredPermission = RolePermissions.Delete;
            // Der Scope muss mit dem der Rolle übereinstimmen. Globale Rollen werden vom Validator bereits abgefangen.
            var requiredScope = role.Scope!;

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to delete this role is required.");
            }
        }
    }
}
