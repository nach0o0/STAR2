using MediatR;
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

namespace Permission.Application.Features.Commands.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleCommandHandler : IRequestHandler<RemovePermissionFromRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;

        public RemovePermissionFromRoleCommandHandler(IRoleRepository roleRepository, IUserPermissionAssignmentRepository assignmentRepository)
        {
            _roleRepository = roleRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task Handle(RemovePermissionFromRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            // --- "LETZTER ADMIN"-PRÜFUNG ---
            if (command.PermissionId == AssignmentPermissions.AssignDirect || command.PermissionId == AssignmentPermissions.AssignRole)
            {
                var affectedScopes = await _assignmentRepository.GetScopesForRoleAssignmentAsync(command.RoleId, cancellationToken);
                foreach (var scope in affectedScopes)
                {
                    var adminCount = await _assignmentRepository.CountUsersWithPermissionInScopeAsync(command.PermissionId, scope, cancellationToken);
                    if (adminCount <= 1)
                    {
                        throw new ValidationException(new Dictionary<string, string[]> {
                            { "PermissionId", new[] { $"Removing this permission would leave the scope '{scope}' without an administrator." } }
                        });
                    }
                }
            }
            // --------------------------------

            // Ruft die Methode auf der Entität auf.
            role.RemovePermission(command.PermissionId);
        }
    }
}
