using MediatR;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetMyTimeEntriesByDateRange
{
    public class GetMyTimeEntriesByDateRangeQueryHandler : IRequestHandler<GetMyTimeEntriesByDateRangeQuery, List<GetMyTimeEntriesByDateRangeQueryResult>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IUserContext _userContext;

        public GetMyTimeEntriesByDateRangeQueryHandler(ITimeEntryRepository timeEntryRepository, IUserContext userContext)
        {
            _timeEntryRepository = timeEntryRepository;
            _userContext = userContext;
        }

        public async Task<List<GetMyTimeEntriesByDateRangeQueryResult>> Handle(GetMyTimeEntriesByDateRangeQuery query, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            var timeEntries = await _timeEntryRepository.GetPersonalAndAnonymizedEntriesAsync(
                currentUser.EmployeeId!.Value,
                query.StartDate,
                query.EndDate,
                query.AccessKeys,
                cancellationToken
            );

            return timeEntries.Select(te => new GetMyTimeEntriesByDateRangeQueryResult(
                te.Id,
                te.EntryDate,
                te.Hours,
                te.Description,
                te.CostObjectId,
                te.IsAnonymized,
                te.AccessKey
            )).OrderBy(te => te.EntryDate).ToList();
        }
    }
}
