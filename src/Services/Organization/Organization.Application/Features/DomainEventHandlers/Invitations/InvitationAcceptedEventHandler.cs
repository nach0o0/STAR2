using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.AddEmployeeToGroup;
using Organization.Application.Features.Commands.AddEmployeeToOrganization;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Organization.Domain.Events.Invitations;
using Shared.Enums;
using Shared.Messages.Events.OrganizationService.Invitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationAcceptedEventHandler : INotificationHandler<InvitationAcceptedEvent>
    {
        private readonly ISender _sender;
        private readonly IPublishEndpoint _publishEndpoint;

        public InvitationAcceptedEventHandler(ISender sender, IPublishEndpoint publishEndpoint)
        {
            _sender = sender;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(InvitationAcceptedEvent notification, CancellationToken cancellationToken)
        {
            var invitation = notification.Invitation;

            // Führe die Zuweisung basierend auf dem Ziel der Einladung durch.
            switch (invitation.TargetEntityType)
            {
                case InvitationTargetEntityType.Organization:
                    await _sender.Send(
                        new AddEmployeeToOrganizationCommand(invitation.InviteeEmployeeId, invitation.TargetEntityId),
                        cancellationToken);
                    break;
                case InvitationTargetEntityType.EmployeeGroup:
                    await _sender.Send(
                        new AddEmployeeToGroupCommand(invitation.InviteeEmployeeId, invitation.TargetEntityId),
                        cancellationToken);
                    break;
                default:
                    throw new NotSupportedException("The specified invitation target type is not supported.");
            }

            // Veröffentliche das Event für z.B. einen NotificationService
            var integrationEvent = new InvitationAcceptedIntegrationEvent
            {
                InvitationId = invitation.Id,
                InviteeEmployeeId = invitation.InviteeEmployeeId,
                TargetEntityId = invitation.TargetEntityId,
                TargetEntityType = invitation.TargetEntityType
            };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
