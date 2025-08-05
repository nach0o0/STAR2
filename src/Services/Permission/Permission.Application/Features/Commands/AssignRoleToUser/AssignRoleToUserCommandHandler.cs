using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using Shared.Application.Exceptions;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Guid>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;

        public AssignRoleToUserCommandHandler(
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUserPermissionAssignmentRepository assignmentRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Guid> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            // Geschäftsregel-Validierung: Prüft, ob die Permissions der Rolle zum Scope passen.
            var targetScopeType = command.Scope.Split(':')[0];
            var permissionsInRole = await _roleRepository.GetPermissionsForRoleAsync(command.RoleId, cancellationToken);

            foreach (var permissionId in permissionsInRole)
            {
                var permission = await _permissionRepository.GetByIdAsync(permissionId, cancellationToken);
                if (permission is not null && !permission.PermittedScopeTypes.Any(link => link.ScopeType == targetScopeType))
                {
                    throw new ValidationException(new Dictionary<string, string[]> {
                    { "RoleId", new[] { $"The role '{role.Name}' contains permissions not valid for the scope type '{targetScopeType}'." } }
                });
                }
            }

            var assignment = UserPermissionAssignment.CreateForRole(command.UserId, command.Scope, command.RoleId);
            await _assignmentRepository.AddAsync(assignment, cancellationToken);

            return assignment.Id;
        }
    }
}
