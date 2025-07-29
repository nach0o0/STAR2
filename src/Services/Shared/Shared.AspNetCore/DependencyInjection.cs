using Microsoft.Extensions.DependencyInjection;
using Shared.AspNetCore.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AspNetCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedAspNetCoreServices(this IServiceCollection services)
        {
            services.AddScoped<GlobalExceptionHandlerMiddleware>();

            return services;
        }
    }
}
