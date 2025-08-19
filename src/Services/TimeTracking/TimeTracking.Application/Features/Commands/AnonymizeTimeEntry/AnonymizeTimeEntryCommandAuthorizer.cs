using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Commands.AnonymizeTimeEntry
{
    public class AnonymizeTimeEntryCommandAuthorizer : ICommandAuthorizer<AnonymizeTimeEntryCommand>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public AnonymizeTimeEntryCommandAuthorizer(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task AuthorizeAsync(AnonymizeTimeEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(command.TimeEntryId, cancellationToken);
            if (timeEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.TimeEntry), command.TimeEntryId);
            }

            // Regel 1: Der Eintrag darf nicht bereits anonymisiert sein.
            if (timeEntry.IsAnonymized)
            {
                throw new ForbiddenAccessException("This time entry has already been anonymized.");
            }

            // Regel 2: Die EmployeeId des Benutzers MUSS mit der des Eintrags übereinstimmen.
            if (!currentUser.EmployeeId.HasValue || timeEntry.EmployeeId != currentUser.EmployeeId)
            {
                throw new ForbiddenAccessException("You are not authorized to anonymize this time entry.");
            }
        }
    }
}
