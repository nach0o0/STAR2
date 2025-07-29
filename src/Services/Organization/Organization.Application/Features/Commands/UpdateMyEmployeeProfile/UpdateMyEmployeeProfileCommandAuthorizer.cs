using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateMyEmployeeProfile
{
    public class UpdateMyEmployeeProfileCommandAuthorizer : ICommandAuthorizer<UpdateMyEmployeeProfileCommand>
    {
        public Task AuthorizeAsync(UpdateMyEmployeeProfileCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (currentUser.EmployeeId == Guid.Empty)
            {
                throw new ForbiddenAccessException("User does not have an employee profile to update.");
            }

            return Task.CompletedTask;
        }
    }
}
