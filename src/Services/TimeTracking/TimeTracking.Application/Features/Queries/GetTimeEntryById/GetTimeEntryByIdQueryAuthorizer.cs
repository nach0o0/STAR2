using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Queries.GetTimeEntryById
{
    public class GetTimeEntryByIdQueryAuthorizer : ICommandAuthorizer<GetTimeEntryByIdQuery>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public GetTimeEntryByIdQueryAuthorizer(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task AuthorizeAsync(GetTimeEntryByIdQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(query.TimeEntryId, cancellationToken);
            if (timeEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.TimeEntry), query.TimeEntryId);
            }

            // Regel 1: Anonymisierte Einträge können NICHT über ihre ID abgerufen werden.
            if (timeEntry.IsAnonymized)
            {
                throw new ForbiddenAccessException("Anonymized time entries can only be accessed via their access key.");
            }

            // Regel 2: Die EmployeeId des Benutzers MUSS mit der des Eintrags übereinstimmen.
            if (!currentUser.EmployeeId.HasValue || timeEntry.EmployeeId != currentUser.EmployeeId)
            {
                throw new ForbiddenAccessException("You are not authorized to view this time entry.");
            }
        }
    }
}
