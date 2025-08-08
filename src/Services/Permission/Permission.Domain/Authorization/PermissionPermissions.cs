using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Authorization
{
    public static class PermissionPermissions
    {
        public const string Read = "permission:read";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Read, "Allows reading available permissions.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup })
        };
    }
}
