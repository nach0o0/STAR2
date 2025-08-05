using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Authorization
{
    public static class RolePermissions
    {
        public const string Create = "role:create";
        public const string Update = "role:update";
        public const string Delete = "role:delete";
        public const string AssignPermission = "role:assign-permission";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating roles.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating roles.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting roles.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (AssignPermission, "Allows assigning permissions to roles.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup })
    };
    }
}
