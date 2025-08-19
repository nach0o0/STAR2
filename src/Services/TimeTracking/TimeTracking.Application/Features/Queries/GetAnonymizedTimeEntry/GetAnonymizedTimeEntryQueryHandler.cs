using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetAnonymizedTimeEntry
{
    public class GetAnonymizedTimeEntryQueryHandler : IRequestHandler<GetAnonymizedTimeEntryQuery, GetAnonymizedTimeEntryQueryResult?>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public GetAnonymizedTimeEntryQueryHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task<GetAnonymizedTimeEntryQueryResult?> Handle(GetAnonymizedTimeEntryQuery query, CancellationToken cancellationToken)
        {
            // Finde den Eintrag ausschließlich über den AccessKey.
            var timeEntry = await _timeEntryRepository.GetByAccessKeyAsync(query.AccessKey, cancellationToken);

            if (timeEntry is null)
            {
                return null; // Wenn kein Eintrag gefunden wird, wird null zurückgegeben.
            }

            return new GetAnonymizedTimeEntryQueryResult(
                timeEntry.Id,
                timeEntry.EntryDate,
                timeEntry.Hours,
                timeEntry.HourlyRate,
                timeEntry.Description,
                timeEntry.CostObjectId,
                timeEntry.EmployeeGroupId
            );
        }
    }
}
