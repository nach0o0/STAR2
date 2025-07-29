using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Authorization
{
    public static class EmployeeGroupPermissions
    {
        public const string Create = "employee-group:create";
        public const string Read = "employee-group:read";
        public const string Update = "employee-group:update";
        public const string Delete = "employee-group:delete";
        public const string Transfer = "employee-group:transfer";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
            {
            (Create, "Allows creating employee groups.", new List<string> { PermittedScopeTypes.Organization }),
            (Read, "Allows reading employee group data.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating employee groups.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting employee groups.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Transfer, "Allows transferring employee groups between organizations.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup })
        };
    }
}
