using CostObject.Application.Features.IntegrationEventConsumers;
using CostObject.Application.Interfaces.Persistence;
using CostObject.Infrastructure.Persistence;
using CostObject.Infrastructure.Persistence.Repositories;
using CostObject.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Options;

namespace CostObject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CostObjectDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AttendanceDbConnection")));

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<CostObjectDbContext>());
            services.AddScoped<ICostObjectRepository, CostObjectRepository>();
            services.AddScoped<ICostObjectRequestRepository, CostObjectRequestRepository>();
            services.AddScoped<IHierarchyDefinitionRepository, HierarchyDefinitionRepository>();
            services.AddScoped<IHierarchyLevelRepository, HierarchyLevelRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
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
