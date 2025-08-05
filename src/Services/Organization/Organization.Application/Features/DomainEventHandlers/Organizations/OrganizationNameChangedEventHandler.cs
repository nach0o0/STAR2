using MediatR;
using Organization.Application.Features.Commands.UpdateEmployeeGroup;
using Organization.Domain.Events.Organizations;

namespace Organization.Application.Features.DomainEventHandlers.Organizations
{
    public class OrganizationNameChangedEventHandler : INotificationHandler<OrganizationNameChangedEvent>
    {
        private readonly ISender _sender;

        public OrganizationNameChangedEventHandler(ISender sender)
        {
            _sender = sender;
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
            var newGroupName = $"{organization.Name} - default group";
            var updateGroupCommand = new UpdateEmployeeGroupCommand(organization.DefaultEmployeeGroupId.Value, newGroupName);
            await _sender.Send(updateGroupCommand, cancellationToken);
        }
    }
}
