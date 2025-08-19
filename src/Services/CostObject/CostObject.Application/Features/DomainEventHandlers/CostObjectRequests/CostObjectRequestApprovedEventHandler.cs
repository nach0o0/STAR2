using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Events.CostObjectRequests;
using MediatR;
using Shared.Domain.Exceptions;

namespace CostObject.Application.Features.DomainEventHandlers.CostObjectRequests
{
    public class CostObjectRequestApprovedEventHandler : INotificationHandler<CostObjectRequestApprovedEvent>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public CostObjectRequestApprovedEventHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task Handle(CostObjectRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            var request = notification.CostObjectRequest;

            // 1. Lade die zugehörige Kostenstelle, die sich im "Pending"-Status befindet.
            var costObject = await _costObjectRepository.GetByIdAsync(request.CostObjectId, cancellationToken);

            if (costObject is null)
            {
                // Dieser Fall sollte nie eintreten, wenn die Daten konsistent sind.
                // Es ist dennoch eine wichtige Absicherung.
                throw new NotFoundException(nameof(Domain.Entities.CostObject), request.CostObjectId);
            }

            // 2. Rufe die Approve-Methode auf der Kostenstellen-Entität auf, um ihren Status zu ändern.
            costObject.Approve();
        }
    }
}
