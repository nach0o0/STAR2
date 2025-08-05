using MassTransit;
using MediatR;
using Permission.Application.Features.Commands.AssignRoleToUser;
using Permission.Domain.Constants;
using Shared.Domain.Authorization;
using Shared.Messages.Events.OrganizationService;

namespace Permission.Application.Features.IntegrationEventConsumers
{
    public class OrganizationCreatedConsumer : IConsumer<OrganizationCreatedIntegrationEvent>
    {
        private readonly ISender _sender;

        public OrganizationCreatedConsumer(ISender sender)
        {
            _sender = sender;
        }

        public Task Consume(ConsumeContext<OrganizationCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            // Greift direkt auf die vordefinierte Rolle zu.
            var adminRole = DefaultRoleDefinitions.OrganizationAdminRole;

            var scope = $"{PermittedScopeTypes.Organization}:{message.OrganizationId}";

            // Erstellt und sendet den Command, um die bestehende Logik wiederzuverwenden.
            var command = new AssignRoleToUserCommand(message.CreatorUserId, adminRole.Id, scope);

            return _sender.Send(command, context.CancellationToken);
        }
    }
}
