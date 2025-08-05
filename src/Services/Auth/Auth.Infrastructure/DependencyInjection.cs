using Auth.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Persistence;
using Auth.Infrastructure.Persistence.Repositories;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Shared.Application.Interfaces.Security;
using Microsoft.Extensions.Options;
using Shared.Options;
using Auth.Infrastructure.Security;
using Auth.Application.Interfaces.Persistence;

namespace Auth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext registrieren
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AuthDbConnection")));

            // Repositories und andere Infrastructure-Services registrieren
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AuthDbContext>());
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSingleton<IBasicTokenService, JwtTokenGenerator>();

            services.AddMassTransit(busConfigurator =>
            {
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
