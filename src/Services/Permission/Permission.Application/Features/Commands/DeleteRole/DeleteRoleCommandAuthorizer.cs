using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DeleteRoleCommandAuthorizer> _logger;

        public DeleteRoleCommandAuthorizer(IRoleRepository roleRepository,
        ILogger<DeleteRoleCommandAuthorizer> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task AuthorizeAsync(DeleteRoleCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[AUTHORIZER] Checking permissions for RoleId {RoleId}", command.RoleId);
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
                _logger.LogWarning("[AUTHORIZER] User lacks permission {Permission} in scope {Scope}", requiredPermission, requiredScope);
                throw new ForbiddenAccessException("Permission to delete this role is required.");
            }
            _logger.LogInformation("[AUTHORIZER] Authorization successful for RoleId {RoleId}", command.RoleId);
        }
    }
}
