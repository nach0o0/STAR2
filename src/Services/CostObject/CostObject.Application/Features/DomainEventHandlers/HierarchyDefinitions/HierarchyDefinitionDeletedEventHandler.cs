using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Events.HierarchyDefinitions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.DomainEventHandlers.HierarchyDefinitions
{
    public class HierarchyDefinitionDeletedEventHandler : INotificationHandler<HierarchyDefinitionDeletedEvent>
    {
        private readonly IHierarchyLevelRepository _hierarchyLevelRepository;

        public HierarchyDefinitionDeletedEventHandler(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            _hierarchyLevelRepository = hierarchyLevelRepository;
        }

        public async Task Handle(HierarchyDefinitionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var definitionId = notification.HierarchyDefinition.Id;

            // 1. Finde alle Ebenen, die zu dieser Definition gehören.
            var levelsToDelete = await _hierarchyLevelRepository.GetByHierarchyDefinitionIdAsync(definitionId, cancellationToken);

            // 2. Lösche jede dieser Ebenen.
            foreach (var level in levelsToDelete)
            {
                // Hier wird bewusst kein "PrepareForDeletion" aufgerufen, da die Validierungslogik
                // (ob die Ebene noch in Benutzung ist) bereits im Command-Validator greift.
                // Das Löschen der Definition ist der übergeordnete Prozess.
                _hierarchyLevelRepository.Delete(level);
            }
        }
    }
}
