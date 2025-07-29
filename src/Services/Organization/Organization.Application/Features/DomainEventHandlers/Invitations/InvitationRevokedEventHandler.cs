using MassTransit;
using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Invitations;
using Shared.Messages.Events.OrganizationService.Invitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationRevokedEventHandler : INotificationHandler<InvitationRevokedEvent>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public InvitationRevokedEventHandler(IInvitationRepository invitationRepository, IPublishEndpoint publishEndpoint)
        {
            _invitationRepository = invitationRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(InvitationRevokedEvent notification, CancellationToken cancellationToken)
        {
            var invitation = notification.Invitation;

            // Führe die Lösch-Operation durch
            _invitationRepository.Delete(invitation);

            // Veröffentliche das Event
            var integrationEvent = new InvitationRevokedIntegrationEvent
            {
                InvitationId = invitation.Id,
                InviteeEmployeeId = invitation.InviteeEmployeeId
            };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
