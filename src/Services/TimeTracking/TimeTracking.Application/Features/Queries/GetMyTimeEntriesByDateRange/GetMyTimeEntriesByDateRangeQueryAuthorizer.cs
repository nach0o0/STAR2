using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetMyTimeEntriesByDateRange
{
    public class GetMyTimeEntriesByDateRangeQueryAuthorizer : ICommandAuthorizer<GetMyTimeEntriesByDateRangeQuery>
    {
        public Task AuthorizeAsync(GetMyTimeEntriesByDateRangeQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("You must have an employee profile to view your time entries.");
            }

            return Task.CompletedTask;
        }
    }
}
