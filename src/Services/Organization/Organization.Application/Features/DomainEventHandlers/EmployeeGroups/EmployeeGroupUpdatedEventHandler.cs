using MassTransit;
using MediatR;
using Organization.Domain.Events.EmployeeGroups;
using Shared.Messages.Events.OrganizationService.EmployeeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupUpdatedEventHandler : INotificationHandler<EmployeeGroupUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeGroupUpdatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(EmployeeGroupUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var group = notification.EmployeeGroup;
            var integrationEvent = new EmployeeGroupUpdatedIntegrationEvent
            {
                EmployeeGroupId = group.Id,
                NewName = group.Name
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
