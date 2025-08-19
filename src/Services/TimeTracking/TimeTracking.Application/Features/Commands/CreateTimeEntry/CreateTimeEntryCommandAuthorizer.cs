using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.CreateTimeEntry
{
    public class CreateTimeEntryCommandAuthorizer : ICommandAuthorizer<CreateTimeEntryCommand>
    {
        public Task AuthorizeAsync(CreateTimeEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Grundlegende Prüfung: Der Benutzer muss ein Mitarbeiter sein, um überhaupt Zeiten zu erfassen.
            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("You must have an employee profile to create time entries.");
            }

            return Task.CompletedTask;
        }
    }
}
