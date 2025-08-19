using MediatR;
using Planning.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByGroup
{
    public class GetPlanningEntriesByGroupQueryHandler : IRequestHandler<GetPlanningEntriesByGroupQuery, List<GetPlanningEntriesByGroupQueryResult>>
    {
        private readonly IPlanningEntryRepository _planningRepository;
        private readonly IOrganizationServiceClient _orgServiceClient;
        private readonly ICostObjectServiceClient _costObjectServiceClient;

        public GetPlanningEntriesByGroupQueryHandler(
            IPlanningEntryRepository planningRepository,
            IOrganizationServiceClient orgServiceClient,
            ICostObjectServiceClient costObjectServiceClient)
        {
            _planningRepository = planningRepository;
            _orgServiceClient = orgServiceClient;
            _costObjectServiceClient = costObjectServiceClient;
        }

        public async Task<List<GetPlanningEntriesByGroupQueryResult>> Handle(GetPlanningEntriesByGroupQuery query, CancellationToken cancellationToken)
        {
            var entries = await _planningRepository.GetByGroupIdAndDateRangeAsync(query.EmployeeGroupId, query.StartDate, query.EndDate, cancellationToken);
            if (!entries.Any())
            {
                return new List<GetPlanningEntriesByGroupQueryResult>();
            }

            // Sammle IDs für die Anreicherung
            var employeeIds = entries.Select(e => e.EmployeeId).Distinct().ToList();
            var costObjectIds = entries.Select(e => e.CostObjectId).Distinct().ToList();

            // Rufe die Namen von den anderen Services ab
            var employees = (await _orgServiceClient.GetEmployeesByIdsAsync(employeeIds, cancellationToken)).ToDictionary(e => e.Id);
            var costObjects = (await _costObjectServiceClient.GetByIdsAsync(costObjectIds, cancellationToken)).ToDictionary(c => c.Id);

            return entries.Select(e => new GetPlanningEntriesByGroupQueryResult(
                e.Id,
                e.EmployeeId,
                employees.TryGetValue(e.EmployeeId, out var emp) ? $"{emp.LastName}, {emp.FirstName}" : "N/A",
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
