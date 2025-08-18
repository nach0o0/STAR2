using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeInfoByEmployeeId
{
    public class GetEmployeeInfoByEmployeeIdQueryAuthorizer : ICommandAuthorizer<GetEmployeeInfoByEmployeeIdQuery>
    {
        public Task AuthorizeAsync(GetEmployeeInfoByEmployeeIdQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Interne Endpunkte, die vom Gateway geschützt werden, benötigen keine weitere Autorisierung.
            return Task.CompletedTask;
        }
    }
}
