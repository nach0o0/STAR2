using CostObject.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectRequestsByGroup
{
    public class GetCostObjectRequestsByGroupQueryHandler : IRequestHandler<GetCostObjectRequestsByGroupQuery, List<GetCostObjectRequestsByGroupQueryResult>>
    {
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly ICostObjectRepository _costObjectRepository;

        public GetCostObjectRequestsByGroupQueryHandler(
            ICostObjectRequestRepository requestRepository,
            ICostObjectRepository costObjectRepository)
        {
            _requestRepository = requestRepository;
            _costObjectRepository = costObjectRepository;
        }

        public async Task<List<GetCostObjectRequestsByGroupQueryResult>> Handle(GetCostObjectRequestsByGroupQuery query, CancellationToken cancellationToken)
        {
            var requests = await _requestRepository.GetByGroupIdAsync(query.EmployeeGroupId, cancellationToken);

            // Filtere nach Status, falls ein Filter im Query übergeben wurde
            var filteredRequests = query.Status.HasValue
                ? requests.Where(r => r.Status == query.Status.Value).ToList()
                : requests;

            if (!filteredRequests.Any())
            {
                return new List<GetCostObjectRequestsByGroupQueryResult>();
            }

            // Hole die zugehörigen Kostenstellen, um deren Namen anzuzeigen
            var costObjectIds = filteredRequests.Select(r => r.CostObjectId).Distinct().ToList();
            var costObjects = (await _costObjectRepository.GetByIdsAsync(costObjectIds, cancellationToken))
                              .ToDictionary(co => co.Id);

            return filteredRequests.Select(r => new GetCostObjectRequestsByGroupQueryResult(
                r.Id,
                r.CostObjectId,
                costObjects.TryGetValue(r.CostObjectId, out var co) ? co.Name : "N/A",
                r.RequesterEmployeeId,
                r.Status,
                r.CreatedAt
            )).OrderByDescending(r => r.CreatedAt).ToList();
        }
    }
}
