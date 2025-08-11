using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net.Http;
using WpfClient.Services.Api.Handlers;

namespace WpfClient.Services.Api.Extensions
{
    public static class RefitClientExtensions
    {
        public static IServiceCollection AddStarApiClient<T>(this IServiceCollection services, string baseAddress)
            where T : class
        {
            Action<HttpClient> configureClient = client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("STAR-WpfClient");
            };

            services.AddRefitClient<T>()
                .ConfigureHttpClient(configureClient)
                .AddHttpMessageHandler<AuthDelegatingHandler>()
                .AddHttpMessageHandler<LoggingDelegatingHandler>();
            return services;
        }
    }
}
