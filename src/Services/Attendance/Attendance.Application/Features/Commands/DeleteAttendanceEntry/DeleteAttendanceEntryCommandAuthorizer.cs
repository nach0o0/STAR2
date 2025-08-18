using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteAttendanceEntry
{
    public class DeleteAttendanceEntryCommandAuthorizer : ICommandAuthorizer<DeleteAttendanceEntryCommand>
    {
        private readonly IAttendanceEntryRepository _entryRepository;

        public DeleteAttendanceEntryCommandAuthorizer(IAttendanceEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task AuthorizeAsync(DeleteAttendanceEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var entry = await _entryRepository.GetByIdAsync(command.AttendanceEntryId, cancellationToken);
            if (entry is null)
            {
                throw new NotFoundException(nameof(AttendanceEntry), command.AttendanceEntryId);
            }

            // Strikte Regel: Nur der Mitarbeiter selbst darf seinen eigenen Eintrag löschen.
            if (currentUser.EmployeeId != entry.EmployeeId)
            {
                throw new ForbiddenAccessException("You are only authorized to delete your own attendance entries.");
            }
        }
    }
}
