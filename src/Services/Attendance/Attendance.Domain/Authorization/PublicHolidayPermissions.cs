using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Authorization
{
    public static class PublicHolidayPermissions
    {
        public const string Create = "public-holiday:create";
        public const string Read = "public-holiday:read";
        public const string Delete = "public-holiday:delete";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating public holidays.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Read, "Allows reading public holidays.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting public holidays.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
