using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Authorization
{
    public static class EmployeePermissions
    {
        public const string Create = "employee:create";
        public const string Read = "employee:read";
        public const string Update = "employee:update";
        public const string Delete = "employee:delete";
        public const string AssignToGroup = "employee:assign-to-group";
        public const string RemoveFromOrganization = "employee:remove-from-organization";
        public const string AssignHouryRate = "employee:assign-hourly-rate";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating employees.", new List<string> { PermittedScopeTypes.Organization }),
            (Read, "Allows reading employee data.", new List<string> { PermittedScopeTypes.Organization }),
            (Update, "Allows updating employees.", new List<string> { PermittedScopeTypes.Organization }),
            (Delete, "Allows deleting employees.", new List<string> { PermittedScopeTypes.Organization }),
            (AssignToGroup, "Allows assigning employees to employee groups.", new List<string> { PermittedScopeTypes.Organization }),
            (RemoveFromOrganization, "Allows removing employees from the organization.", new List<string> { PermittedScopeTypes.Organization }),
            (AssignHouryRate, "Allows assigning hourly rates to employees for employee groups.", new List<string> { PermittedScopeTypes.Organization })
        };
    }
}