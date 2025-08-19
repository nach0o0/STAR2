using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Commands.UpdateTimeEntry
{
    public class UpdateTimeEntryCommandAuthorizer : ICommandAuthorizer<UpdateTimeEntryCommand>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public UpdateTimeEntryCommandAuthorizer(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task AuthorizeAsync(UpdateTimeEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(command.TimeEntryId, cancellationToken);
            if (timeEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.TimeEntry), command.TimeEntryId);
            }

            // Fall 1: Der Eintrag ist anonymisiert.
            if (timeEntry.IsAnonymized)
            {
                // Der Benutzer MUSS einen AccessKey angeben, und dieser muss mit dem des Eintrags übereinstimmen.
                if (command.AccessKey.HasValue && timeEntry.AccessKey == command.AccessKey)
                {
                    return; // Zugriff gewährt
                }
            }
            // Fall 2: Der Eintrag ist NICHT anonymisiert.
            else
            {
                // Die EmployeeId des Benutzers MUSS mit der des Eintrags übereinstimmen.
                if (currentUser.EmployeeId.HasValue && timeEntry.EmployeeId == currentUser.EmployeeId)
                {
                    return; // Zugriff gewährt
                }
            }

            // Wenn keine der Bedingungen erfüllt ist, wird der Zugriff verweigert.
            throw new ForbiddenAccessException("You are not authorized to update this time entry.");
        }
    }
}
