using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetMyCostObjectRequests
{
    public class GetMyCostObjectRequestsQueryAuthorizer : ICommandAuthorizer<GetMyCostObjectRequestsQuery>
    {
        public Task AuthorizeAsync(GetMyCostObjectRequestsQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("You must have an employee profile to view your requests.");
            }

            if (!currentUser.EmployeeGroupIds.Contains(query.EmployeeGroupId))
            {
                throw new ForbiddenAccessException("You can only view requests for an employee group you are a member of.");
            }

            return Task.CompletedTask;
        }
    }
}
