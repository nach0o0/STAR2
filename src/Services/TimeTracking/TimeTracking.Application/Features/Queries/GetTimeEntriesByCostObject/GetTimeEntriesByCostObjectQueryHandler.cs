using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByCostObject
{
    public class GetTimeEntriesByCostObjectQueryHandler : IRequestHandler<GetTimeEntriesByCostObjectQuery, List<GetTimeEntriesByCostObjectQueryResult>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public GetTimeEntriesByCostObjectQueryHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task<List<GetTimeEntriesByCostObjectQueryResult>> Handle(GetTimeEntriesByCostObjectQuery query, CancellationToken cancellationToken)
        {
            var timeEntries = await _timeEntryRepository.GetByCostObjectAndDateRangeAsync(
                query.CostObjectId,
                query.StartDate,
                query.EndDate,
                cancellationToken
            );

            return timeEntries.Select(te => new GetTimeEntriesByCostObjectQueryResult(
                te.Id,
                te.EmployeeId!.Value,
                te.EntryDate,
                te.Hours,
                te.Description
            )).OrderBy(te => te.EntryDate).ToList();
        }
    }
}
