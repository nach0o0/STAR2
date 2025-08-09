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
        public static IServiceCollection AddMediatrPipelineBehaviors(this IServiceCollection services)
        {
            // Registriert die Pipeline-Behaviors in der korrekten Ausführungsreihenfolge
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            return services;
        }
    }
}
