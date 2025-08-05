using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Authorization
{
    public static class AssignmentPermissions
    {
        public const string AssignRole = "permission:assign-role";
        public const string AssignDirect = "permission:assign-direct";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (AssignRole, "Allows assigning roles to users.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (AssignDirect, "Allows assigning direct permissions to users.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup })
        };
    }
}
