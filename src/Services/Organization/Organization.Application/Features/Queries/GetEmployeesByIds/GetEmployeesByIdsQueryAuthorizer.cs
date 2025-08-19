using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByIds
{
    public class GetEmployeesByIdsQueryAuthorizer : ICommandAuthorizer<GetEmployeesByIdsQuery>
    {
        public Task AuthorizeAsync(GetEmployeesByIdsQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Die aufrufenden Dienste sind vertrauenswürdig.
            return Task.CompletedTask;
        }
    }
}
