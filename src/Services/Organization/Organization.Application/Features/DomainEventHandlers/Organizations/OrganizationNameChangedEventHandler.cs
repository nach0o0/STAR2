using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.UpdateEmployeeGroup;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Organizations;
using Shared.Domain.Exceptions;
using Shared.Messages.Events.OrganizationService.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Organizations
{
    public class OrganizationNameChangedEventHandler : INotificationHandler<OrganizationNameChangedEvent>
    {
        private readonly ISender _sender;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrganizationNameChangedEventHandler(ISender sender, IPublishEndpoint publishEndpoint)
        {
            _sender = sender;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(OrganizationNameChangedEvent notification, CancellationToken cancellationToken)
        {
            var organization = notification.Organization;

            if (!organization.DefaultEmployeeGroupId.HasValue)
            {
                // Sollte nie passieren, da das Event nur dann ausgelöst wird.
                return;
            }

            // Aktualisiere den Namen der Gruppe.
            var newGroupName = $"Default group: {organization.Name}";
            var updateGroupCommand = new UpdateEmployeeGroupCommand(organization.DefaultEmployeeGroupId.Value, newGroupName);
            await _sender.Send(updateGroupCommand, cancellationToken);

            // Veröffentliche das Integration Event
            var integrationEvent = new OrganizationUpdatedIntegrationEvent
            {
                OrganizationId = organization.Id,
                NewName = organization.Name
            };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
