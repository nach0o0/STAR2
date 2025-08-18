using Attendance.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateOrUpdateAttendanceEntry
{
    public class CreateOrUpdateAttendanceEntryCommandAuthorizer : ICommandAuthorizer<CreateOrUpdateAttendanceEntryCommand>
    {
        public Task AuthorizeAsync(CreateOrUpdateAttendanceEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Der Benutzer trägt für sich selbst etwas ein.
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                return Task.CompletedTask; // Jeder Mitarbeiter darf seine eigene Anwesenheit eintragen.
            }

            throw new ForbiddenAccessException("You are not authorized to create or update attendance entries for other employees.");
        }
    }
}
