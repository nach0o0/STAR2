using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateMyEmployeeProfile
{
    public class CreateMyEmployeeProfileCommandAuthorizer : ICommandAuthorizer<CreateMyEmployeeProfileCommand>
    {
        public Task AuthorizeAsync(CreateMyEmployeeProfileCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (currentUser.EmployeeId != Guid.Empty)
            {
                throw new ForbiddenAccessException("An employee profile already exists for this user.");
            }

            return Task.CompletedTask;
        }
    }
}
