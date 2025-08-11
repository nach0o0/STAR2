using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedApplicationServices(this IServiceCollection services, Assembly applicationAssembly)
        {
            // Registriere MediatR und alle Handler aus dem spezifischen Anwendungs-Projekt
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }
    }
}
