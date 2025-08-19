using MediatR;
using Planning.Application.Common.Dtos;
using Planning.Application.Common.Enums;
using Planning.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningSummary
{
    public class GetPlanningSummaryQueryHandler : IRequestHandler<GetPlanningSummaryQuery, List<GetPlanningSummaryQueryResult>>
    {
        private readonly IPlanningEntryRepository _planningRepository;
        private readonly ICostObjectServiceClient _costObjectServiceClient;
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetPlanningSummaryQueryHandler(
            IPlanningEntryRepository planningRepository,
            ICostObjectServiceClient costObjectServiceClient,
            IOrganizationServiceClient organizationServiceClient)
        {
            _planningRepository = planningRepository;
            _costObjectServiceClient = costObjectServiceClient;
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<List<GetPlanningSummaryQueryResult>> Handle(GetPlanningSummaryQuery query, CancellationToken cancellationToken)
        {
            List<PlanningSummaryDto> summaryDtos;
            switch (query.GroupBy)
            {
                case SummaryGroupBy.CostObject:
                    summaryDtos = await _planningRepository.GetSummaryByCostObjectAsync(query.EmployeeGroupId, query.StartDate, query.EndDate, cancellationToken);
                    if (!summaryDtos.Any()) return new List<GetPlanningSummaryQueryResult>();

                    var costObjectIds = summaryDtos.Select(s => s.GroupingId).ToList();
                    var costObjects = (await _costObjectServiceClient.GetByIdsAsync(costObjectIds, cancellationToken)).ToDictionary(co => co.Id);

                    return summaryDtos.Select(s => new GetPlanningSummaryQueryResult(
                        s.GroupingId,
                        costObjects.TryGetValue(s.GroupingId, out var co) ? co.Name : "N/A",
                        s.TotalHours
                    )).OrderBy(r => r.GroupingName).ToList();

                case SummaryGroupBy.Employee:
                    summaryDtos = await _planningRepository.GetSummaryByEmployeeAsync(query.EmployeeGroupId, query.StartDate, query.EndDate, cancellationToken);
                    if (!summaryDtos.Any()) return new List<GetPlanningSummaryQueryResult>();

                    var employeeIds = summaryDtos.Select(s => s.GroupingId).ToList();
                    var employees = (await _organizationServiceClient.GetEmployeesByIdsAsync(employeeIds, cancellationToken)).ToDictionary(e => e.Id);

                    return summaryDtos.Select(s => new GetPlanningSummaryQueryResult(
                        s.GroupingId,
                        employees.TryGetValue(s.GroupingId, out var emp) ? $"{emp.LastName}, {emp.FirstName}" : "N/A",
                        s.TotalHours
                    )).OrderBy(r => r.GroupingName).ToList();

                default:
                    throw new ArgumentOutOfRangeException(nameof(query.GroupBy), "Invalid group by option provided.");
            }
        }
    }
}
