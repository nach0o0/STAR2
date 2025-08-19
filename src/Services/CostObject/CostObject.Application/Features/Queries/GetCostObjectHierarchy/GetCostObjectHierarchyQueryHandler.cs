using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectHierarchy
{
    public class GetCostObjectHierarchyQueryHandler : IRequestHandler<GetCostObjectHierarchyQuery, GetCostObjectHierarchyQueryResult?>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetCostObjectHierarchyQueryHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task<GetCostObjectHierarchyQueryResult?> Handle(GetCostObjectHierarchyQuery query, CancellationToken cancellationToken)
        {
            var rootObject = await _costObjectRepository.GetByIdAsync(query.RootCostObjectId, cancellationToken);

            if (rootObject is null)
            {
                return null;
            }

            // Starte den rekursiven Aufbau der Hierarchie
            return await BuildHierarchyNodeAsync(rootObject, cancellationToken);
        }

        private async Task<GetCostObjectHierarchyQueryResult> BuildHierarchyNodeAsync(Domain.Entities.CostObject costObject, CancellationToken cancellationToken)
        {
            var children = await _costObjectRepository.GetChildrenAsync(costObject.Id, cancellationToken);
            var childNodes = new List<GetCostObjectHierarchyQueryResult>();

            foreach (var child in children)
            {
                // Rekursiver Aufruf für jeden Nachkommen
                childNodes.Add(await BuildHierarchyNodeAsync(child, cancellationToken));
            }

            return new GetCostObjectHierarchyQueryResult(
                costObject.Id,
                costObject.Name,
                costObject.ApprovalStatus,
                childNodes
            );
        }
    }
}
