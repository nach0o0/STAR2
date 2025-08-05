using MassTransit;
using MediatR;
using Permission.Application.Features.Commands.AssignRoleToUser;
using Permission.Domain.Constants;
using Shared.Domain.Authorization;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.IntegrationEventConsumers
{
    public class EmployeeGroupCreatedConsumer : IConsumer<EmployeeGroupCreatedIntegrationEvent>
    {
        private readonly ISender _sender;

        public EmployeeGroupCreatedConsumer(ISender sender)
        {
            _sender = sender;
        }

        public Task Consume(ConsumeContext<EmployeeGroupCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            var adminRole = DefaultRoleDefinitions.EmployeeGroupAdminRole;

            var scope = $"{PermittedScopeTypes.EmployeeGroup}:{message.EmployeeGroupId}";

            var command = new AssignRoleToUserCommand(message.CreatorUserId, adminRole.Id, scope);

            return _sender.Send(command, context.CancellationToken);
        }
    }
}
