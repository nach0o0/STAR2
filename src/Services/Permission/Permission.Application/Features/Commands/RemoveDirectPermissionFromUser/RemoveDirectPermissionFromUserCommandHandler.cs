using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Authorization;
using Shared.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemoveDirectPermissionFromUser
{
    public class RemoveDirectPermissionFromUserCommandHandler : IRequestHandler<RemoveDirectPermissionFromUserCommand>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;

        public RemoveDirectPermissionFromUserCommandHandler(IUserPermissionAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task Handle(RemoveDirectPermissionFromUserCommand command, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.FindDirectPermissionAssignmentAsync(
                command.UserId, command.PermissionId, command.Scope, cancellationToken);

            if (assignment is null) return;

            // --- "LETZTER ADMIN"-PRÜFUNG ---
            // Prüfe nur, wenn es sich um eine kritische Management-Berechtigung handelt.
            if (command.PermissionId == AssignmentPermissions.AssignDirect)
            {
                var adminCount = await _assignmentRepository.CountUsersWithPermissionInScopeAsync(
                    command.PermissionId, command.Scope, cancellationToken);

                // Wenn dies der letzte Benutzer mit dieser Berechtigung ist, verhindere die Löschung.
                if (adminCount <= 1)
                {
                    throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "UserId", new[] { "Cannot remove the last user with permission management rights from this scope." } }
                });
                }
            }
            // --------------------------------

            assignment.PrepareForDeletion();
            _assignmentRepository.Delete(assignment);
        }
    }
}
