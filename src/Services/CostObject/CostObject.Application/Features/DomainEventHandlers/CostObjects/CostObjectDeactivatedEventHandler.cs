using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Events.CostObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.DomainEventHandlers.CostObjects
{
    public class CostObjectDeactivatedEventHandler : INotificationHandler<CostObjectDeactivatedEvent>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public CostObjectDeactivatedEventHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task Handle(CostObjectDeactivatedEvent notification, CancellationToken cancellationToken)
        {
            var deactivatedParent = notification.CostObject;

            // Hole alle direkten und indirekten Nachkommen der deaktivierten Kostenstelle.
            var descendants = await _costObjectRepository.GetAllDescendantsAsync(deactivatedParent.Id, cancellationToken);

            foreach (var descendant in descendants)
            {
                if (!descendant.ValidTo.HasValue || descendant.ValidTo.Value > deactivatedParent.ValidTo)
                {
                    descendant.Deactivate(deactivatedParent.ValidTo!.Value);
                }
            }
        }
    }
}
