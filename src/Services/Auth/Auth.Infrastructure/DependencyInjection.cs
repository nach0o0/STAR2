using Auth.Application.Interfaces.Infrastructure;
using Auth.Application.Interfaces.Persistence;
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
            services.AddSingleton<IBasicTokenService, JwtTokenGenerator>();

            return services;
        }
    }
}
