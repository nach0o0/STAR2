using MassTransit;
using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Events.Roles;
using Shared.Messages.Events.PermissionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.DomainEventHandlers.Roles
{
    public class PermissionRemovedFromRoleEventHandler : INotificationHandler<PermissionRemovedFromRoleEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public PermissionRemovedFromRoleEventHandler(
            IUserPermissionAssignmentRepository assignmentRepository,
            IPublishEndpoint publishEndpoint)
        {
            _assignmentRepository = assignmentRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(PermissionRemovedFromRoleEvent notification, CancellationToken cancellationToken)
        {
            // 1. Finde alle Benutzer, denen diese Rolle zugewiesen ist.
            var affectedUserIds = await _assignmentRepository.GetUserIdsByRoleIdAsync(notification.RoleId, cancellationToken);

            // 2. Veröffentliche für jeden betroffenen Benutzer ein Event, um den SessionService zu informieren.
            foreach (var userId in affectedUserIds)
            {
                var integrationEvent = new UserPermissionsChangedIntegrationEvent { UserId = userId };
                await _publishEndpoint.Publish(integrationEvent, cancellationToken);
            }
        }
    }
}
