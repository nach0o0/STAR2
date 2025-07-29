using MassTransit;
using MediatR;
using Organization.Domain.Events.Invitations;
using Shared.Messages.Events.OrganizationService.Invitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationCreatedEventHandler : INotificationHandler<InvitationCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public InvitationCreatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(InvitationCreatedEvent notification, CancellationToken cancellationToken)
        {
            var invitation = notification.Invitation;
            var integrationEvent = new InvitationCreatedIntegrationEvent
            {
                InvitationId = invitation.Id,
                InviterEmployeeId = invitation.InviterEmployeeId,
                InviteeEmployeeId = invitation.InviteeEmployeeId,
                TargetEntityId = invitation.TargetEntityId,
                TargetEntityType = invitation.TargetEntityType
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
