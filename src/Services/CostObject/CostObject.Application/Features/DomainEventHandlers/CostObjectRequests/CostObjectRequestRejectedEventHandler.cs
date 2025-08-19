using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Events.CostObjectRequests;
using MassTransit;
using MediatR;
using Shared.Messages.Events.CostObjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.DomainEventHandlers.CostObjectRequests
{
    public class CostObjectRequestRejectedEventHandler : INotificationHandler<CostObjectRequestRejectedEvent>
    {
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CostObjectRequestRejectedEventHandler(
            ICostObjectRepository costObjectRepository,
            IPublishEndpoint publishEndpoint)
        {
            _costObjectRepository = costObjectRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(CostObjectRequestRejectedEvent notification, CancellationToken cancellationToken)
        {
            var request = notification.CostObjectRequest;

            // 1. Finde die zugehörige Kostenstelle, die im "Pending"-Status erstellt wurde.
            var costObjectToDelete = await _costObjectRepository.GetByIdAsync(request.CostObjectId, cancellationToken);

            if (costObjectToDelete != null)
            {
                // 2. Entferne die abgelehnte Kostenstelle aus der Datenbank.
                costObjectToDelete.PrepareForDeletion();
                _costObjectRepository.Delete(costObjectToDelete);
            }

            // 3. Wenn eine Umbuchungs-ID angegeben wurde, veröffentliche ein Integration Event.
            if (notification.ReassignmentCostObjectId.HasValue)
            {
                var integrationEvent = new CostObjectEntriesReassignedIntegrationEvent
                {
                    SourceCostObjectId = request.CostObjectId,
                    DestinationCostObjectId = notification.ReassignmentCostObjectId.Value
                };
                await _publishEndpoint.Publish(integrationEvent, cancellationToken);
            }
        }
    }
}
