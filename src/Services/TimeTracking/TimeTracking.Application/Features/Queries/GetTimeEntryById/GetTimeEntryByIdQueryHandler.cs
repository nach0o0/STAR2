using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetTimeEntryById
{
    public class GetTimeEntryByIdQueryHandler : IRequestHandler<GetTimeEntryByIdQuery, GetTimeEntryByIdQueryResult?>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public GetTimeEntryByIdQueryHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task<GetTimeEntryByIdQueryResult?> Handle(GetTimeEntryByIdQuery query, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(query.TimeEntryId, cancellationToken);

            if (timeEntry is null || timeEntry.EmployeeId is null)
            {
                return null;
            }

            return new GetTimeEntryByIdQueryResult(
                timeEntry.Id,
                timeEntry.EntryDate,
                timeEntry.Hours,
                timeEntry.HourlyRate,
                timeEntry.Description,
                timeEntry.CostObjectId,
                timeEntry.EmployeeGroupId,
                timeEntry.EmployeeId.Value
            );
        }
    }
}
