using MediatR;
using Planning.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByEmployee
{
    public class GetPlanningEntriesByEmployeeQueryHandler : IRequestHandler<GetPlanningEntriesByEmployeeQuery, List<GetPlanningEntriesByEmployeeQueryResult>>
    {
        private readonly IPlanningEntryRepository _planningRepository;
        private readonly ICostObjectServiceClient _costObjectServiceClient;

        public GetPlanningEntriesByEmployeeQueryHandler(
            IPlanningEntryRepository planningRepository,
            ICostObjectServiceClient costObjectServiceClient)
        {
            _planningRepository = planningRepository;
            _costObjectServiceClient = costObjectServiceClient;
        }

        public async Task<List<GetPlanningEntriesByEmployeeQueryResult>> Handle(GetPlanningEntriesByEmployeeQuery query, CancellationToken cancellationToken)
        {
            var entries = await _planningRepository.GetByEmployeeIdAndGroupIdAndDateRangeAsync(
                query.EmployeeId,
                query.EmployeeGroupId,
                query.StartDate,
                query.EndDate,
                cancellationToken);

            if (!entries.Any())
            {
                return new List<GetPlanningEntriesByEmployeeQueryResult>();
            }

            var costObjectIds = entries.Select(e => e.CostObjectId).Distinct().ToList();
            var costObjects = (await _costObjectServiceClient.GetByIdsAsync(costObjectIds, cancellationToken)).ToDictionary(c => c.Id);

            return entries.Select(e => new GetPlanningEntriesByEmployeeQueryResult(
                e.Id,
                e.CostObjectId,
                costObjects.TryGetValue(e.CostObjectId, out var co) ? co.Name : "N/A",
                e.PlannedHours,
                e.PlanningPeriodStart,
                e.PlanningPeriodEnd,
                e.Notes
            )).ToList();
        }
    }
}
