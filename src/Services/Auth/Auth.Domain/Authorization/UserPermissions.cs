using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Authorization
{
    public static class UserPermissions
    {
        public const string PrivilegedResetPassword = "user:privileged-reset-password";

        public static IReadOnlyCollection<(string Id, string Description, List<string> PermittedScopeTypes)> AllPermissions { get; } = new[]
        {
            (PrivilegedResetPassword, "Allows an administrator to reset a user's password.", new List<string> { PermittedScopeTypes.Global, PermittedScopeTypes.Organization })
        };
    }
}
