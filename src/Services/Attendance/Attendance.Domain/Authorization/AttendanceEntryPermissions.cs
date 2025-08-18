using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Authorization
{
    public static class AttendanceEntryPermissions
    {
        public const string Create = "attendance-entry:create";
        public const string Read = "attendance-entry:read";
        public const string Update = "attendance-entry:update";
        public const string Delete = "attendance-entry:delete";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating attendance entries for others.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Read, "Allows reading attendance entries of others.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating attendance entries of others.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting attendance entries of others.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup })
        };
    }
}
