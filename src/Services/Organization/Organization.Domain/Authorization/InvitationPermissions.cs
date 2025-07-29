using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Authorization
{
    public static class InvitationPermissions
    {
        public const string Create = "invitation:create";
        public const string Read = "invitation:read";
        public const string Revoke = "invitation:revoke";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (
                Create,
                "Allows creating invitations.",
                new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }
            ),
            (
                Read,
                "Allows reading invitations.",
                new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }
            ),
            (
                Revoke,
                "Allows revoking invitations.",
                new List<string> { PermittedScopeTypes.Organization, PermittedScopeTypes.EmployeeGroup }
            )
        };
    }
}
