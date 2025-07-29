using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Clients
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IOrganizationServiceClient, OrganizationServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiClients:OrganizationServiceUrl"]!);
            });

            services.AddHttpClient<IPermissionRegistrationClient, PermissionServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiClients:PermissionServiceUrl"]!);
            });

            services.AddHttpClient<IPermissionQueryClient, PermissionServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiClients:PermissionServiceUrl"]!);
            });

            return services;
        }
    }
}
