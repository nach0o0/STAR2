using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.AddEmployeeToGroup;
using Organization.Application.Features.Commands.AddEmployeeToOrganization;
using Organization.Domain.Events.Invitations;
using Shared.Enums;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationAcceptedEventHandler : INotificationHandler<InvitationAcceptedEvent>
    {
        private readonly ISender _sender;

        public InvitationAcceptedEventHandler(ISender sender)
        {
            _sender = sender;
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
        }
    }
}
