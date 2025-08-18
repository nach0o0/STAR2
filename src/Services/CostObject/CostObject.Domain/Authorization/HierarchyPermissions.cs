using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Authorization
{
    public static class HierarchyPermissions
    {
        public const string Manage = "hierarchy:manage";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Manage, "Allows managing hierarchy definitions and their levels.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
