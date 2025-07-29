using MassTransit;
using MediatR;
using Organization.Domain.Events.EmployeeGroups;
using Shared.Application.Interfaces.Security;
using Shared.Messages.Events.OrganizationService.EmployeeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupCreatedEventHandler : INotificationHandler<EmployeeGroupCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUserContext _userContext;

        public EmployeeGroupCreatedEventHandler(IPublishEndpoint publishEndpoint, IUserContext userContext)
        {
            _publishEndpoint = publishEndpoint;
            _userContext = userContext;
        }

        public Task Handle(EmployeeGroupCreatedEvent notification, CancellationToken cancellationToken)
        {
            var group = notification.EmployeeGroup;
            var currentUser = _userContext.GetCurrentUser()!;
            var integrationEvent = new EmployeeGroupCreatedIntegrationEvent
            {
                EmployeeGroupId = group.Id,
                Name = group.Name,
                LeadingOrganizationId = group.LeadingOrganizationId,
                CreatorUserId = currentUser.UserId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
