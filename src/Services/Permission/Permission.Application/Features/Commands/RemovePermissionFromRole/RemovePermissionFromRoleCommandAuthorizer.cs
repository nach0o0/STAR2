using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Authorization;
using Permission.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleCommandAuthorizer : ICommandAuthorizer<RemovePermissionFromRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public RemovePermissionFromRoleCommandAuthorizer(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task AuthorizeAsync(RemovePermissionFromRoleCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            var requiredPermission = RolePermissions.AssignPermission;
            var requiredScope = role.Scope ?? PermittedScopeTypes.Global;

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to remove permissions from this role is required.");
            }
        }
    }
}
