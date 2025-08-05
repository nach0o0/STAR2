using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Authorization;
using Permission.Domain.Entities;
using Shared.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;

        public RemoveRoleFromUserCommandHandler(IUserPermissionAssignmentRepository assignmentRepository, IRoleRepository roleRepository)
        {
            _assignmentRepository = assignmentRepository;
            _roleRepository = roleRepository;
        }

        public async Task Handle(RemoveRoleFromUserCommand command, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.FindRoleAssignmentAsync(
                command.UserId, command.RoleId, command.Scope, cancellationToken);

            if (assignment is null)
            {
                return; // Zuweisung existiert nicht, nichts zu tun.
            }

            // --- "LETZTER ADMIN"-PRÜFUNG ---
            var permissionsInRole = await _roleRepository.GetPermissionsForRoleAsync(command.RoleId, cancellationToken);
            if (permissionsInRole.Contains(AssignmentPermissions.AssignDirect) || permissionsInRole.Contains(AssignmentPermissions.AssignRole))
            {
                var adminCount = await _assignmentRepository.CountUsersWithPermissionInScopeAsync(
                    AssignmentPermissions.AssignDirect, command.Scope, cancellationToken);

                if (adminCount <= 1)
                {
                    var isThisUserTheLastAdmin = await _assignmentRepository.IsUserTheLastAdminForPermissionAsync(
                        command.UserId, AssignmentPermissions.AssignDirect, command.Scope, cancellationToken);

                    if (isThisUserTheLastAdmin)
                    {
                        throw new ValidationException(new Dictionary<string, string[]> {
                        { "RoleId", new[] { "Cannot remove role from the last user with permission management rights in this scope." } }
                    });
                    }
                }
            }
            // --------------------------------

            assignment.PrepareForDeletion();
            _assignmentRepository.Delete(assignment);
        }
    }
}
