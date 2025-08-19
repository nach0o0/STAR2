using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByIds
{
    public class GetCostObjectsByIdsQueryAuthorizer : ICommandAuthorizer<GetCostObjectsByIdsQuery>
    {
        public Task AuthorizeAsync(GetCostObjectsByIdsQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            return Task.CompletedTask; // Zugriff gewährt für jeden authentifizierten Aufrufer
        }
    }
}
