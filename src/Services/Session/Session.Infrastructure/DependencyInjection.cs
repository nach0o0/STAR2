using Auth.Application.Interfaces.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Session.Application.Interfaces.Infrastructure;
using Session.Application.Interfaces.Persistence;
using Session.Infrastructure.Persistence.Repositories;
using Session.Infrastructure.Persistence;
using Session.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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

            return services;
        }
    }
}
