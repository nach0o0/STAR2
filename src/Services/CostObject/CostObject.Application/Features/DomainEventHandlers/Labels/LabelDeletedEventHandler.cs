using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Events.Labels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.DomainEventHandlers.Labels
{
    public class LabelDeletedEventHandler : INotificationHandler<LabelDeletedEvent>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public LabelDeletedEventHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task Handle(LabelDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedLabelId = notification.Label.Id;

            // 1. Finde alle Kostenstellen, die dieses Label noch verwenden.
            var affectedCostObjects = await _costObjectRepository.GetByLabelIdAsync(deletedLabelId, cancellationToken);

            // 2. Setze bei jeder betroffenen Kostenstelle die LabelId auf null.
            foreach (var costObject in affectedCostObjects)
            {
                costObject.Update(name: null, parentCostObjectId: null, hierarchyLevelId: null, labelId: null); // Setzt LabelId auf null
            }
        }
    }
}
