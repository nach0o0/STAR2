using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMyPendingInvitations
{
    public class GetMyPendingInvitationsQueryAuthorizer : ICommandAuthorizer<GetMyPendingInvitationsQuery>
    {
        public Task AuthorizeAsync(GetMyPendingInvitationsQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("You must have an employee profile to view invitations.");
            }

            return Task.CompletedTask;
        }
    }
}
