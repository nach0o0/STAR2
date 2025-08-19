using MediatR;
using Planning.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByCostObject
{
    public class GetPlanningEntriesByCostObjectQueryHandler : IRequestHandler<GetPlanningEntriesByCostObjectQuery, List<GetPlanningEntriesByCostObjectQueryResult>>
    {
        private readonly IPlanningEntryRepository _planningRepository;
        private readonly IOrganizationServiceClient _orgServiceClient;

        public GetPlanningEntriesByCostObjectQueryHandler(
            IPlanningEntryRepository planningRepository,
            IOrganizationServiceClient orgServiceClient)
        {
            _planningRepository = planningRepository;
            _orgServiceClient = orgServiceClient;
        }

        public async Task<List<GetPlanningEntriesByCostObjectQueryResult>> Handle(GetPlanningEntriesByCostObjectQuery query, CancellationToken cancellationToken)
        {
            var entries = await _planningRepository.GetByCostObjectIdAndDateRangeAsync(query.CostObjectId, query.StartDate, query.EndDate, cancellationToken);
            if (!entries.Any())
            {
                return new List<GetPlanningEntriesByCostObjectQueryResult>();
            }

            var employeeIds = entries.Select(e => e.EmployeeId).Distinct().ToList();
            var employees = (await _orgServiceClient.GetEmployeesByIdsAsync(employeeIds, cancellationToken)).ToDictionary(e => e.Id);

            return entries.Select(e => new GetPlanningEntriesByCostObjectQueryResult(
                e.Id,
                e.EmployeeId,
                employees.TryGetValue(e.EmployeeId, out var emp) ? $"{emp.LastName}, {emp.FirstName}" : "N/A",
                e.PlannedHours,
                e.PlanningPeriodStart,
                e.PlanningPeriodEnd,
                e.Notes
            )).ToList();
        }
    }
}
