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
    public class InvitationDeclinedEventHandler : INotificationHandler<InvitationDeclinedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public InvitationDeclinedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(InvitationDeclinedEvent notification, CancellationToken cancellationToken)
        {
            var invitation = notification.Invitation;
            var integrationEvent = new InvitationDeclinedIntegrationEvent
            {
                InvitationId = invitation.Id,
                InviterEmployeeId = invitation.InviterEmployeeId,
                InviteeEmployeeId = invitation.InviteeEmployeeId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
