using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateCostObjectRequest
{
    public class CreateCostObjectRequestCommandAuthorizer : ICommandAuthorizer<CreateCostObjectRequestCommand>
    {
        public Task AuthorizeAsync(CreateCostObjectRequestCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Prüfung 1: Hat der Benutzer überhaupt ein Mitarbeiterprofil?
            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("You must have an employee profile to create a cost object request.");
            }

            // Prüfung 2: Ist der Mitarbeiter Mitglied der EmployeeGroup, für die der Antrag gestellt wird?
            if (!currentUser.EmployeeGroupIds.Contains(command.EmployeeGroupId))
            {
                throw new ForbiddenAccessException("You are not authorized to create a cost object request for this employee group.");
            }

            return Task.CompletedTask;
        }
    }
}
