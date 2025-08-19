using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Events.HierarchyLevels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.DomainEventHandlers.HierarchyLevels
{
    public class HierarchyLevelDeletedEventHandler : INotificationHandler<HierarchyLevelDeletedEvent>
    {
        private readonly IHierarchyDefinitionRepository _hierarchyDefinitionRepository;

        public HierarchyLevelDeletedEventHandler(IHierarchyDefinitionRepository hierarchyDefinitionRepository)
        {
            _hierarchyDefinitionRepository = hierarchyDefinitionRepository;
        }

        public async Task Handle(HierarchyLevelDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedLevelId = notification.HierarchyLevel.Id;

            // 1. Finde alle Hierarchie-Definitionen, die diese Ebene als "RequiredBookingLevel" verwenden.
            var affectedDefinitions = await _hierarchyDefinitionRepository.FindByRequiredBookingLevelIdAsync(deletedLevelId, cancellationToken);

            // 2. Setze bei jeder betroffenen Definition die Verknüpfung auf null.
            foreach (var definition in affectedDefinitions)
            {
                definition.ClearRequiredBookingLevel();
            }
        }
    }
}
