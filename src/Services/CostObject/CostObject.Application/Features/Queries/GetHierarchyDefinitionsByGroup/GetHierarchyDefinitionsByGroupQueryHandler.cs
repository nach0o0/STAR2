using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetHierarchyDefinitionsByGroup
{
    public class GetHierarchyDefinitionsByGroupQueryHandler : IRequestHandler<GetHierarchyDefinitionsByGroupQuery, List<GetHierarchyDefinitionsByGroupQueryResult>>
    {
        private readonly IHierarchyDefinitionRepository _definitionRepository;

        public GetHierarchyDefinitionsByGroupQueryHandler(IHierarchyDefinitionRepository definitionRepository)
        {
            _definitionRepository = definitionRepository;
        }

        public async Task<List<GetHierarchyDefinitionsByGroupQueryResult>> Handle(GetHierarchyDefinitionsByGroupQuery query, CancellationToken cancellationToken)
        {
            var definitions = await _definitionRepository.GetByGroupIdAsync(query.EmployeeGroupId, cancellationToken);

            return definitions.Select(def => new GetHierarchyDefinitionsByGroupQueryResult(
                def.Id,
                def.Name,
                def.RequiredBookingLevelId
            )).ToList();
        }
    }
}
