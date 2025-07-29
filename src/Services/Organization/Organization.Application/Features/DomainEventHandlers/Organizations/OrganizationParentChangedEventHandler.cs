using MassTransit;
using MediatR;
using Organization.Domain.Events.Organizations;
using Shared.Messages.Events.OrganizationService.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Organizations
{
    public class OrganizationParentChangedEventHandler : INotificationHandler<OrganizationParentChangedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrganizationParentChangedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(OrganizationParentChangedEvent notification, CancellationToken cancellationToken)
        {
            var organization = notification.Organization;

            // Veröffentliche das Integration Event
            var integrationEvent = new OrganizationHierarchyChangedIntegrationEvent
            {
                OrganizationId = organization.Id,
                NewParentId = organization.ParentOrganizationId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
