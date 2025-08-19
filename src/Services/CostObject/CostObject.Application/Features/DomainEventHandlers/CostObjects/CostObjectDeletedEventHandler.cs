using CostObject.Domain.Events.CostObjects;
using MassTransit;
using MediatR;
using Shared.Messages.Events.CostObjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.DomainEventHandlers.CostObjects
{
    public class CostObjectDeletedEventHandler : INotificationHandler<CostObjectDeletedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CostObjectDeletedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(CostObjectDeletedEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new CostObjectDeletedIntegrationEvent
            {
                CostObjectId = notification.CostObject.Id
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
