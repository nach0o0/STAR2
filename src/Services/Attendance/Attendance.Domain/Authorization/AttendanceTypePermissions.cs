using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Authorization
{
    public static class AttendanceTypePermissions
    {
        public const string Create = "attendance-type:create";
        public const string Read = "attendance-type:read";
        public const string Update = "attendance-type:update";
        public const string Delete = "attendance-type:delete";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating attendance types.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Read, "Allows reading attendance types.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating attendance types.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting attendance types.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
