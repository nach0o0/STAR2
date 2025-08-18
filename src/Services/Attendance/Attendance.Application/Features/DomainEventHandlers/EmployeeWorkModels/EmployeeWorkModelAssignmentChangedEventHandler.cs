using Attendance.Domain.Events.EmployeeWorkModels;
using MassTransit;
using MediatR;
using Shared.Messages.Events.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.DomainEventHandlers.EmployeeWorkModels
{
    public class EmployeeWorkModelAssignmentChangedEventHandler :
        INotificationHandler<EmployeeWorkModelAssignedEvent>,
        INotificationHandler<EmployeeWorkModelUnassignedEvent>,
        INotificationHandler<EmployeeWorkModelUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeWorkModelAssignmentChangedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(EmployeeWorkModelAssignedEvent notification, CancellationToken cancellationToken)
        {
            return PublishIntegrationEvent(notification.Assignment.EmployeeId, cancellationToken);
        }

        public Task Handle(EmployeeWorkModelUnassignedEvent notification, CancellationToken cancellationToken)
        {
            return PublishIntegrationEvent(notification.EmployeeId, cancellationToken);
        }

        public Task Handle(EmployeeWorkModelUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return PublishIntegrationEvent(notification.Assignment.EmployeeId, cancellationToken);
        }

        private Task PublishIntegrationEvent(Guid employeeId, CancellationToken cancellationToken)
        {
            var integrationEvent = new EmployeeWorkModelAssignmentChangedIntegrationEvent
            {
                EmployeeId = employeeId
            };
            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
