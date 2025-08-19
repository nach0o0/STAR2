using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Authorization
{
    public static class CostObjectPermissions
    {
        public const string Create = "cost-object:create";
        public const string Read = "cost-object:read";
        public const string Update = "cost-object:update";
        public const string Deactivate = "cost-object:deactivate";
        public const string Approve = "cost-object:approve";
        public const string Sync = "cost-object:sync";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating cost objects.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Read, "Allows reading cost object data.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating cost objects.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Deactivate, "Allows deactivating cost objects.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Approve, "Allows approving or rejecting cost object requests.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Sync, "Allows synchronizing top-level cost objects.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
