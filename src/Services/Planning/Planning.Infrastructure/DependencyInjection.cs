using Planning.Application.Interfaces.Persistence;
using Planning.Infrastructure.Persistence;
using Planning.Infrastructure.Persistence.Repositories;
using Planning.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Options;

namespace Planning.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PlanningDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PlanningDbConnection")));

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<PlanningDbContext>());
            services.AddScoped<ICostObjectRepository, CostObjectRepository>();
            services.AddScoped<ICostObjectRequestRepository, CostObjectRequestRepository>();
            services.AddScoped<IHierarchyDefinitionRepository, HierarchyDefinitionRepository>();
            services.AddScoped<IHierarchyLevelRepository, HierarchyLevelRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<EmployeeEmployeeGroupAssignmentChangedIntegrationEventConsumer>();

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
