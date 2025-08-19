using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Domain.Authorization
{
    public static class PlanningPermissions
    {
        public const string Read = "planning:read";
        public const string Write = "planning:write";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Read, "Allows reading planning entries within an employee group.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Write, "Allows creating, updating, and deleting planning entries within an employee group.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
