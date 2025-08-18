using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Authorization
{
    public static class WorkModelPermissions
    {
        public const string Create = "work-model:create";
        public const string Read = "work-model:read";
        public const string Update = "work-model:update";
        public const string Delete = "work-model:delete";
        public const string Assign = "work-model:assign";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating work models.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Read, "Allows reading work models.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Update, "Allows updating work models.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Delete, "Allows deleting work models.", new List<string> { PermittedScopeTypes.EmployeeGroup }),
            (Assign, "Allows assigning work models to employees.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
