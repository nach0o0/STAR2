using MediatR;
using Permission.Application.Features.Commands.RemovePermissionFromRole;
using Permission.Application.Features.Commands.RemoveRoleFromUser;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Events.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.DomainEventHandlers.Roles
{
    public class RoleDeletedEventHandler : INotificationHandler<RoleDeletedEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISender _sender;

        public RoleDeletedEventHandler(
            IUserPermissionAssignmentRepository assignmentRepository,
            IRoleRepository roleRepository,
            ISender sender)
        {
            _assignmentRepository = assignmentRepository;
            _roleRepository = roleRepository;
            _sender = sender;
        }

        public async Task Handle(RoleDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedRole = notification.Role;

            // 1. Finde alle Berechtigungen, die dieser Rolle zugeordnet waren.
            var permissionIds = await _roleRepository.GetPermissionsForRoleAsync(deletedRole.Id, cancellationToken);

            // 2. Sende für jede Berechtigung einen Command, um die Verknüpfung zu lösen.
            foreach (var permissionId in permissionIds)
            {
                await _sender.Send(new RemovePermissionFromRoleCommand(deletedRole.Id, permissionId), cancellationToken);
            }

            // 3. Finde alle Zuweisungen dieser Rolle an Benutzer.
            var assignments = await _assignmentRepository.GetAssignmentsForRoleAsync(deletedRole.Id, cancellationToken);

            // 4. Sende für jede Zuweisung einen Command, um sie sauber zu entfernen.
            //    Dies löst dann auch das UserPermissionsChangedIntegrationEvent aus.
            foreach (var assignment in assignments)
            {
                await _sender.Send(
                    new RemoveRoleFromUserCommand(assignment.UserId, deletedRole.Id, assignment.Scope),
                    cancellationToken);
            }

            // 5. Finde alle Rollen, die diese Rolle als Vorlage verwenden, und löse die Verknüpfung.
            var derivedRoles = await _roleRepository.GetRolesByBaseRoleIdAsync(deletedRole.Id, cancellationToken);
            foreach (var derivedRole in derivedRoles)
            {
                derivedRole.ClearBaseRole(); // Ruft eine neue Methode auf der Entität auf.
            }
        }
    }
}
