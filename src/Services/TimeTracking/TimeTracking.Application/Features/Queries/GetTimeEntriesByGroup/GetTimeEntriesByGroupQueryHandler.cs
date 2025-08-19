using MediatR;
using Shared.Application.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByGroup
{
    public class GetTimeEntriesByGroupQueryHandler : IRequestHandler<GetTimeEntriesByGroupQuery, List<GetTimeEntriesByGroupQueryResult>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ICostObjectServiceClient _costObjectServiceClient;

        public GetTimeEntriesByGroupQueryHandler(ITimeEntryRepository timeEntryRepository, ICostObjectServiceClient costObjectServiceClient)
        {
            _timeEntryRepository = timeEntryRepository;
            _costObjectServiceClient = costObjectServiceClient;
        }

        public async Task<List<GetTimeEntriesByGroupQueryResult>> Handle(GetTimeEntriesByGroupQuery query, CancellationToken cancellationToken)
        {
            var timeEntries = await _timeEntryRepository.GetByGroupAndDateRangeAsync(
                query.EmployeeGroupId,
                query.StartDate,
                query.EndDate,
                cancellationToken
            );

            if (!timeEntries.Any())
            {
                return new List<GetTimeEntriesByGroupQueryResult>();
            }

            // Hole die Namen der Kostenstellen über den API-Client
            var costObjectIds = timeEntries.Where(te => te.CostObjectId.HasValue).Select(te => te.CostObjectId!.Value).Distinct().ToList();
            var costObjects = (await _costObjectServiceClient.GetByIdsAsync(costObjectIds, cancellationToken))
                              .ToDictionary(co => co.Id);

            return timeEntries.Select(te => new GetTimeEntriesByGroupQueryResult(
                te.Id,
                te.EmployeeId!.Value, // In dieser Abfrage immer vorhanden
                te.EntryDate,
                te.Hours,
                te.Description,
                te.CostObjectId,
                te.CostObjectId.HasValue && costObjects.TryGetValue(te.CostObjectId.Value, out var co) ? co.Name : "N/A"
            )).OrderBy(te => te.EntryDate).ToList();
        }
    }
}
