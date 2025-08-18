using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Authorization
{
    public static class CostObjectRequestPermissions
    {
        public const string Read = "cost-object-request:read";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Read, "Allows reading cost object requests.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
