using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetTopLevelCostObjectsByGroup
{
    public class GetTopLevelCostObjectsByGroupQueryHandler : IRequestHandler<GetTopLevelCostObjectsByGroupQuery, List<GetTopLevelCostObjectsByGroupQueryResult>>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetTopLevelCostObjectsByGroupQueryHandler(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task<List<GetTopLevelCostObjectsByGroupQueryResult>> Handle(GetTopLevelCostObjectsByGroupQuery query, CancellationToken cancellationToken)
        {
            var costObjects = await _costObjectRepository.GetTopLevelByGroupAsync(
                query.EmployeeGroupId,
                query.ApprovedOnly,
                query.ActiveOnly,
                cancellationToken
            );

            return costObjects.Select(co => new GetTopLevelCostObjectsByGroupQueryResult(
                co.Id,
                co.Name,
                co.ApprovalStatus
            )).ToList();
        }
    }
}
