using MediatR;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Common.Dtos;
using TimeTracking.Application.Common.Enums;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetTimeTrackingSummary
{
    public class GetTimeTrackingSummaryQueryHandler : IRequestHandler<GetTimeTrackingSummaryQuery, List<GetTimeTrackingSummaryQueryResult>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ICostObjectServiceClient _costObjectServiceClient;
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetTimeTrackingSummaryQueryHandler(
            ITimeEntryRepository timeEntryRepository,
            ICostObjectServiceClient costObjectServiceClient,
            IOrganizationServiceClient organizationServiceClient)
        {
            _timeEntryRepository = timeEntryRepository;
            _costObjectServiceClient = costObjectServiceClient;
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<List<GetTimeTrackingSummaryQueryResult>> Handle(GetTimeTrackingSummaryQuery query, CancellationToken cancellationToken)
        {
            List<TimeSummaryDto> summaryDtos;

            switch (query.GroupBy)
            {
                case SummaryGroupBy.CostObject:
                    summaryDtos = await _timeEntryRepository.GetSummaryByCostObjectAsync(query.EmployeeGroupId, query.StartDate, query.EndDate, cancellationToken);
                    if (!summaryDtos.Any()) return new List<GetTimeTrackingSummaryQueryResult>();

                    var costObjectIds = summaryDtos.Select(s => s.GroupingId).ToList();
                    var costObjects = (await _costObjectServiceClient.GetByIdsAsync(costObjectIds, cancellationToken))
                                      .ToDictionary(co => co.Id);

                    return summaryDtos.Select(s => new GetTimeTrackingSummaryQueryResult(
                        s.GroupingId,
                        costObjects.TryGetValue(s.GroupingId, out var co) ? co.Name : "N/A",
                        s.TotalHours
                    )).OrderBy(r => r.GroupingName).ToList();
                    

                case SummaryGroupBy.Employee:
                    summaryDtos = await _timeEntryRepository.GetSummaryByEmployeeAsync(query.EmployeeGroupId, query.StartDate, query.EndDate, cancellationToken);
                    if (!summaryDtos.Any()) return new List<GetTimeTrackingSummaryQueryResult>();

                    var employeeIds = summaryDtos.Select(s => s.GroupingId).ToList();
                    var employees = (await _organizationServiceClient.GetEmployeesByIdsAsync(employeeIds, cancellationToken))
                                    .ToDictionary(e => e.Id);

                    return summaryDtos.Select(s => new GetTimeTrackingSummaryQueryResult(
                        s.GroupingId,
                        employees.TryGetValue(s.GroupingId, out var emp) ? $"{emp.LastName}, {emp.FirstName}" : "N/A",
                        s.TotalHours
                    )).OrderBy(r => r.GroupingName).ToList();

                default:
                    throw new InvalidOperationException("Invalid group by option.");
            }
             // Fallback für CostObject
            return new List<GetTimeTrackingSummaryQueryResult>();
            
        }
    }
}
