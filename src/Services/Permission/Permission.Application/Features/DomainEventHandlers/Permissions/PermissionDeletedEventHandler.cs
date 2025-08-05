using MassTransit;
using MediatR;
using Permission.Application.Features.Commands.RemoveDirectPermissionFromUser;
using Permission.Application.Features.Commands.RemovePermissionFromRole;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Events.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.DomainEventHandlers.Permissions
{
    public class PermissionDeletedEventHandler : INotificationHandler<PermissionDeletedEvent>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly ISender _sender;

        public PermissionDeletedEventHandler(
            IRoleRepository roleRepository,
            IUserPermissionAssignmentRepository assignmentRepository,
            ISender sender)
        {
            _roleRepository = roleRepository;
            _assignmentRepository = assignmentRepository;
            _sender = sender;
        }

        public async Task Handle(PermissionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedPermissionId = notification.Permission.Id;

            // 1. Finde alle Rollen, denen diese Berechtigung zugewiesen war.
            var affectedRoles = await _roleRepository.GetRolesByPermissionIdAsync(deletedPermissionId, cancellationToken);

            // 2. Sende für jede betroffene Rolle einen Command, um die Berechtigung zu entfernen.
            //    Dies stellt sicher, dass alle abhängigen Prozesse (wie das Senden von
            //    UserPermissionsChangedIntegrationEvent) korrekt ausgelöst werden.
            foreach (var roleId in affectedRoles)
            {
                await _sender.Send(new RemovePermissionFromRoleCommand(roleId, deletedPermissionId), cancellationToken);
            }

            // 3. Finde alle direkten Zuweisungen dieser Berechtigung an Benutzer.
            var affectedDirectAssignments = await _assignmentRepository.GetDirectAssignmentsByPermissionIdAsync(deletedPermissionId, cancellationToken);

            // 4. Sende für jede direkte Zuweisung einen Command, um sie zu entfernen.
            foreach (var assignment in affectedDirectAssignments)
            {
                await _sender.Send(new RemoveDirectPermissionFromUserCommand(assignment.UserId, deletedPermissionId, assignment.Scope), cancellationToken);
            }
        }
    }
}
