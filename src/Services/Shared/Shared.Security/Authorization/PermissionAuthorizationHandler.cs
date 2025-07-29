using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Security.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissionsByScopeClaim = context.User.FindFirst("permissions_by_scope")?.Value;
            if (string.IsNullOrEmpty(permissionsByScopeClaim))
            {
                return Task.CompletedTask;
            }

            var permissionsByScope = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(permissionsByScopeClaim);
            if (permissionsByScope is null)
            {
                return Task.CompletedTask;
            }

            // Prüft, ob die Berechtigung in irgendeiner der Scope-Listen vorkommt.
            if (permissionsByScope.Values.SelectMany(p => p).Any(p => p == requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
