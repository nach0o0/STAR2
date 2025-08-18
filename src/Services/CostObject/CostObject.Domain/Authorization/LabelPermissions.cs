using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Authorization
{
    public static class LabelPermissions
    {
        public const string Create = "label:create";
        public const string Read = "label:read";
        public const string Update = "label:update";
        public const string Delete = "label:delete";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating labels for cost objects.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Read, "Allows reading labels.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating labels.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting labels.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
