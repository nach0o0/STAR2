using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetActiveCostObjectsByGroup
{
    public class GetActiveCostObjectsByGroupQueryAuthorizer : ICommandAuthorizer<GetActiveCostObjectsByGroupQuery>
    {
        public Task AuthorizeAsync(GetActiveCostObjectsByGroupQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("You must have an employee profile to perform this action.");
            }

            // Strikte Prüfung: Nur Mitglieder der angefragten Gruppe dürfen diese Daten sehen.
            if (!currentUser.EmployeeGroupIds.Contains(query.EmployeeGroupId))
            {
                throw new ForbiddenAccessException("You must be a member of this employee group to view its active cost objects.");
            }

            return Task.CompletedTask;
        }
    }
}
