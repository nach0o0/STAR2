using MassTransit;
using MediatR;
using Permission.Domain.Events.UserPermissionAssignments;
using Shared.Messages.Events.PermissionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.DomainEventHandlers.UserPermissionAssignments
{
    public class UserPermissionAssignmentCreatedEventHandler : INotificationHandler<UserPermissionAssignmentCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserPermissionAssignmentCreatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(UserPermissionAssignmentCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Erstelle das Integration Event, das den SessionService informiert.
            var integrationEvent = new UserPermissionsChangedIntegrationEvent { UserId = notification.Assignment.UserId };

            // Veröffentliche das Event an RabbitMQ.
            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
