using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Authorization
{
    public static class HourlyRatePermissions
    {
        public const string Create = "hourly-rate:create";
        public const string Read = "hourly-rate:read";
        public const string Update = "hourly-rate:update";
        public const string Delete = "hourly-rate:delete";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating hourly rates.", new List<string> { PermittedScopeTypes.Organization }),
            (Read, "Allows reading hourly rate data.", new List<string> { PermittedScopeTypes.Organization }),
            (Update, "Allows updating hourly rates.", new List<string> { PermittedScopeTypes.Organization }),
            (Delete, "Allows deleting hourly rates.", new List<string> { PermittedScopeTypes.Organization })
        };
    }
}
