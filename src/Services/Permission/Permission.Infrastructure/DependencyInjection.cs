using Auth.Application.Interfaces.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Permission.Application.Interfaces.Persistence;
using Permission.Infrastructure.Persistence.Repositories;
using Permission.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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

            return services;
        }
    }
}
