using TimeTracking.Application.Interfaces.Persistence;
using TimeTracking.Infrastructure.Persistence;
using TimeTracking.Infrastructure.Persistence.Repositories;
using TimeTracking.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Options;
using TimeTracking.Application.Features.IntegrationEventConsumers;

namespace TimeTracking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<TimeTrackingDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("TimeTrackingDbConnection")));

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TimeTrackingDbContext>());
            services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<CostObjectDeletedConsumer>();
                busConfigurator.AddConsumer<CostObjectEntriesReassignedConsumer>();
                busConfigurator.AddConsumer<EmployeeGroupDeletedConsumer>();

                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    // Hole die stark typisierten Optionen aus dem DI-Container
                    var options = context.GetRequiredService<IOptions<MessageBrokerOptions>>().Value;

                    cfg.Host(options.Host, "/", h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
