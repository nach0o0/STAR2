using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByIds
{
    public class GetCostObjectsByIdsQueryHandler : IRequestHandler<GetCostObjectsByIdsQuery, List<GetCostObjectsByIdsQueryResult>>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetCostObjectsByIdsQueryHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task<List<GetCostObjectsByIdsQueryResult>> Handle(GetCostObjectsByIdsQuery query, CancellationToken cancellationToken)
        {
            var costObjects = await _costObjectRepository.GetByIdsAsync(query.CostObjectIds, cancellationToken);

            return costObjects.Select(co => new GetCostObjectsByIdsQueryResult(
                co.Id,
                co.Name,
                co.EmployeeGroupId
            )).ToList();
        }
    }
}
