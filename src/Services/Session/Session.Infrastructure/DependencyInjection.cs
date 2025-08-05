using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Session.Application.Features.IntegrationEventConsumers;
using Session.Application.Interfaces.Infrastructure;
using Session.Application.Interfaces.Persistence;
using Session.Infrastructure.Persistence;
using Session.Infrastructure.Persistence.Repositories;
using Session.Infrastructure.Security;
using Session.Infrastructure.Services;
using Shared.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Options;

namespace Session.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext registrieren
            services.AddDbContext<SessionDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SessionDbConnection")));

            // Unit of Work und Repositories registrieren
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SessionDbContext>());
            services.AddScoped<IActiveSessionRepository, ActiveSessionRepository>();
            services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();

            // Infrastructure-Services registrieren
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddScoped<IEnrichedTokenService, EnrichedTokenService>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<UserPermissionsChangedConsumer>();
                busConfigurator.AddConsumer<UserPasswordChangedConsumer>();
                busConfigurator.AddConsumer<UserAccountDeletedConsumer>();
                busConfigurator.AddConsumer<EmployeeMembershipChangedConsumer>();
                busConfigurator.AddConsumer<UserAssignedToEmployeeConsumer>();
                busConfigurator.AddConsumer<UserUnassignedFromEmployeeConsumer>();

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
