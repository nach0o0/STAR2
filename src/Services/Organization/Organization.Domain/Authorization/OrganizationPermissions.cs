using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Authorization
{
    public static class OrganizationPermissions
    {
        public const string Create = "organization:create";
        public const string Read = "organization:read";
        public const string Update = "organization:update";
        public const string Delete = "organization:delete";
        public const string ReassignToParent = "organization:reassign-to-parent";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (Create, "Allows creating organizations.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization }),
            (Read, "Allows reading organization data.", new List<string> { PermittedScopeTypes.Organization }),
            (Update, "Allows updating organizations.", new List<string> { PermittedScopeTypes.Organization }),
            (Delete, "Allows deleting organizations.", new List<string> { PermittedScopeTypes.Organization }),
            (ReassignToParent, "Allows reassigning to the parent of an organization.", new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.Global })
        };
    }
}
