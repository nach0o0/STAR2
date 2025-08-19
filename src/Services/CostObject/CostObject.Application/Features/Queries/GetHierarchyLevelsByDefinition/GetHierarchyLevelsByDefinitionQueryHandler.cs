using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetHierarchyLevelsByDefinition
{
    public class GetHierarchyLevelsByDefinitionQueryHandler : IRequestHandler<GetHierarchyLevelsByDefinitionQuery, List<GetHierarchyLevelsByDefinitionQueryResult>>
    {
        private readonly IHierarchyLevelRepository _hierarchyLevelRepository;

        public GetHierarchyLevelsByDefinitionQueryHandler(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            _hierarchyLevelRepository = hierarchyLevelRepository;
        }

        public async Task<List<GetHierarchyLevelsByDefinitionQueryResult>> Handle(GetHierarchyLevelsByDefinitionQuery query, CancellationToken cancellationToken)
        {
            var levels = await _hierarchyLevelRepository.GetByHierarchyDefinitionIdAsync(query.HierarchyDefinitionId, cancellationToken);

            return levels.Select(level => new GetHierarchyLevelsByDefinitionQueryResult(
                level.Id,
                level.Name,
                level.Depth
            )).OrderBy(l => l.Depth).ToList();
        }
    }
}
