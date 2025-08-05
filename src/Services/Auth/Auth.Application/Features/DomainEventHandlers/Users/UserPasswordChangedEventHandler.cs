using Auth.Domain.Events.Users;
using MassTransit;
using MediatR;
using Shared.Messages.Events.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.DomainEventHandlers.Users
{
    public class UserPasswordChangedEventHandler : INotificationHandler<UserPasswordChangedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserPasswordChangedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(UserPasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserPasswordChangedIntegrationEvent { UserId = notification.UserId };
            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
