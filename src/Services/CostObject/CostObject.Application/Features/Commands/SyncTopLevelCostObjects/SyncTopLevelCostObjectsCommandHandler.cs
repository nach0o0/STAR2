using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.SyncTopLevelCostObjects
{
    public class SyncTopLevelCostObjectsCommandHandler : IRequestHandler<SyncTopLevelCostObjectsCommand>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public SyncTopLevelCostObjectsCommandHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task Handle(SyncTopLevelCostObjectsCommand command, CancellationToken cancellationToken)
        {
            var existingTopLevelObjects = await _costObjectRepository.GetTopLevelCostObjectsByGroupAsync(command.EmployeeGroupId, cancellationToken);
            var existingNames = existingTopLevelObjects.ToDictionary(co => co.Name, co => co);
            var providedNames = new HashSet<string>(command.Names);

            // 1. Deaktivieren: Finde Objekte, die in der DB, aber nicht mehr in der neuen Liste sind.
            var objectsToDeactivate = existingTopLevelObjects.Where(co => !providedNames.Contains(co.Name)).ToList();
            foreach (var topLevelObject in objectsToDeactivate)
            {
                var deactivationDate = command.ValidFrom.AddDays(-1);

                // Deaktiviere das Top-Level-Objekt
                if (!topLevelObject.ValidTo.HasValue || topLevelObject.ValidTo.Value > deactivationDate)
                {
                    topLevelObject.Deactivate(deactivationDate);
                }

                // Lade alle Nachkommen und deaktiviere sie ebenfalls
                var descendants = await _costObjectRepository.GetAllDescendantsAsync(topLevelObject.Id, cancellationToken);
                foreach (var descendant in descendants)
                {
                    if (!descendant.ValidTo.HasValue || descendant.ValidTo.Value > deactivationDate)
                    {
                        descendant.Deactivate(deactivationDate);
                    }
                }
            }

            // 2. Erstellen: Finde Namen, die in der neuen Liste, aber noch nicht in der DB sind.
            var namesToCreate = providedNames.Where(name => !existingNames.ContainsKey(name)).ToList();
            foreach (var name in namesToCreate)
            {
                var newCostObject = new Domain.Entities.CostObject(
                    name,
                    command.EmployeeGroupId,
                    null, // Kein Parent, da Top-Level
                    command.TopHierarchyLevelId,
                    null, // Kein Label initial
                    command.ValidFrom,
                    isApprovedDirectly: true // Direkt genehmigt
                );
                await _costObjectRepository.AddAsync(newCostObject, cancellationToken);
            }
        }
    }
}
