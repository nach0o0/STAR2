using Auth.Application.Interfaces.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Organization.Application.Interfaces.Persistence;
using Organization.Infrastructure.Persistence.Repositories;
using Organization.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure.Security;
using Shared.Application.Interfaces.Security;
using MassTransit;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Organization.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OrganizationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("OrganizationDbConnection")));

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<OrganizationDbContext>());
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IEmployeeGroupRepository, EmployeeGroupRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IHourlyRateRepository, HourlyRateRepository>();

            services.AddScoped<IUserContext, UserContext>();

            services.AddMassTransit(busConfigurator =>
            {
                // ... AddConsumers ...

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
