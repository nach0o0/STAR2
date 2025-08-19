using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Domain.Authorization
{
    public static class TimeEntryPermissions
    {
        // Berechtigung für Manager/Admins, alle Einträge einer Gruppe zu sehen
        public const string ReadAll = "time-entry:read-all";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (ReadAll, "Allows reading all time entries within an employee group.", new List<string> { PermittedScopeTypes.EmployeeGroup })
        };
    }
}
