using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Application.Interfaces.Clients;

namespace Shared.AspNetCore.Extensions
{
    public static class PermissionRegistrationExtensions
    {
        public static async Task RegisterServicePermissionsAsync(
            this IApplicationBuilder app,
            string serviceName,
            IEnumerable<(string Id, string Description)> permissions)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<object>>();

            try
            {
                var client = scope.ServiceProvider.GetRequiredService<IPermissionRegistrationClient>();
                await client.RegisterPermissionsAsync(permissions);
                logger.LogInformation("Successfully registered permissions for service: {ServiceName}", serviceName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to register permissions for service: {ServiceName}", serviceName);
            }
        }
    }
}
