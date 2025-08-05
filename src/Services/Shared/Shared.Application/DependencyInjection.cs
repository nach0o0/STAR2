using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
