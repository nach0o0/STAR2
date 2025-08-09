using MediatR;
using Microsoft.Extensions.Logging;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Authorization;
using Permission.Domain.Entities;
using Shared.Application.Exceptions;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly ILogger<DeleteRoleCommandHandler> _logger;

        public DeleteRoleCommandHandler(
            IRoleRepository roleRepository,
            IUserPermissionAssignmentRepository assignmentRepository, ILogger<DeleteRoleCommandHandler> logger)
        {
            _roleRepository = roleRepository;
            _assignmentRepository = assignmentRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[HANDLER] Executing DeleteRoleCommandHandler for RoleId {RoleId}", command.RoleId);
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            // --- "LETZTER ADMIN"-PRÜFUNG ---
            var permissionsInRole = await _roleRepository.GetPermissionsForRoleAsync(command.RoleId, cancellationToken);
            if (permissionsInRole.Contains(AssignmentPermissions.AssignDirect) || permissionsInRole.Contains(AssignmentPermissions.AssignRole))
            {
                var affectedScopes = await _assignmentRepository.GetScopesForRoleAssignmentAsync(command.RoleId, cancellationToken);
                foreach (var scope in affectedScopes)
                {
                    var adminCount = await _assignmentRepository.CountUsersWithPermissionInScopeAsync(
                        AssignmentPermissions.AssignDirect, scope, cancellationToken);

                    if (adminCount <= 1)
                    {
                        throw new ValidationException(new Dictionary<string, string[]> {
                        { "RoleId", new[] { $"Deleting this role would leave the scope '{scope}' without an administrator." } }
                    });
                    }
                }
            }
            // --------------------------------

            role.PrepareForDeletion();
            _roleRepository.Delete(role);
        }
    }
}
