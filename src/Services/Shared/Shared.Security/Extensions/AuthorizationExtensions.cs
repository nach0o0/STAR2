using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Security.Extensions
{
public static class AuthorizationExtensions
{
    public static IServiceCollection AddPermissionsAuthorization(
        this IServiceCollection services, 
        IEnumerable<(string Id, string Description, List<string> PermittedScopeTypes)> permissions)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            // Erstelle für jede Berechtigung in der übergebenen Liste eine Policy.
            foreach (var (permission, _, _) in permissions)
            {
                options.AddPolicy(permission, policy => 
                    policy.AddRequirements(new PermissionRequirement(permission)));
            }
        });
        
        return services;
    }
}
}
