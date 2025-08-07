using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Api.Handlers;

namespace WpfClient.Services.Api.Extensions
{
    public static class RefitClientExtensions
    {
        public static IServiceCollection AddStarApiClient<T>(this IServiceCollection services, string baseAddress)
            where T : class
        {
            services.AddRefitClient<T>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseAddress))
                .AddHttpMessageHandler<AuthDelegatingHandler>();
            return services;
        }
    }
}
