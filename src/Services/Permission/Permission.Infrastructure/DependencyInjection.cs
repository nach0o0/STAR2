using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Permission.Application.Features.IntegrationEventConsumers;
using Permission.Application.Interfaces.Persistence;
using Permission.Infrastructure.Persistence;
using Permission.Infrastructure.Persistence.Repositories;
using Permission.Infrastructure.Security;
using Shared.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Options;

namespace Permission.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PermissionDbConnection")));

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<PermissionDbContext>());
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserPermissionAssignmentRepository, UserPermissionAssignmentRepository>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<OrganizationCreatedConsumer>();
                busConfigurator.AddConsumer<EmployeeGroupCreatedConsumer>();
                busConfigurator.AddConsumer<UserAccountDeletedConsumer>();
                busConfigurator.AddConsumer<OrganizationDeletedConsumer>();
                busConfigurator.AddConsumer<EmployeeGroupDeletedConsumer>();
                busConfigurator.AddConsumer<EmployeeOrganizationAssignmentChangedConsumer>();
                busConfigurator.AddConsumer<EmployeeEmployeeGroupAssignmentChangedConsumer>();

                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
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
