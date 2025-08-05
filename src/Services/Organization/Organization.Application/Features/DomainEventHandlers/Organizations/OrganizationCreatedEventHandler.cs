using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.CreateEmployeeGroup;
using Organization.Domain.Events.Organizations;
using Shared.Application.Interfaces.Security;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.Organizations
{
    public class OrganizationCreatedEventHandler : INotificationHandler<OrganizationCreatedEvent>
    {
        private readonly ISender _sender;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUserContext _userContext;

        public OrganizationCreatedEventHandler(ISender sender, IPublishEndpoint publishEndpoint, IUserContext userContext)
        {
            _sender = sender;
            _publishEndpoint = publishEndpoint;
            _userContext = userContext;
        }

        public async Task Handle(OrganizationCreatedEvent notification, CancellationToken cancellationToken)
        {
            var organization = notification.Organization;
            var currentUser = _userContext.GetCurrentUser()!;

            // 1. Erstelle die zugehörige DefaultEmployeeGroup.
            var defaultGroupName = $"{organization.Name} - default group";
            var createGroupCommand = new CreateEmployeeGroupCommand(defaultGroupName, organization.Id);
            var defaultGroupId = await _sender.Send(createGroupCommand, cancellationToken);

            // 2. Verknüpfe die Gruppe mit der Organisation.
            organization.SetDefaultEmployeeGroup(defaultGroupId);

            // 3. Veröffentliche das Integration Event
            var integrationEvent = new OrganizationCreatedIntegrationEvent
            {
                OrganizationId = organization.Id,
                Name = organization.Name,
                CreatorUserId = currentUser.UserId
            };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
