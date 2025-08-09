using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Permission.Application.Features.Commands.DeleteRole;
using Shared.Application;
using Shared.Application.Behaviors;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 1️⃣ MediatR registrieren (scannt automatisch alle Handler in dieser Assembly)
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DeleteRoleCommand).Assembly);
            });

            // 2️⃣ Pipeline Behaviors in der richtigen Reihenfolge
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            // 3️⃣ FluentValidation: alle Validatoren in der Assembly automatisch registrieren
            services.AddValidatorsFromAssemblyContaining<DeleteRoleCommandValidator>();

            // 4️⃣ ICommandAuthorizer<T>: alle Authorizer automatisch registrieren
            services.Scan(scan => scan
                .FromAssemblyOf<DeleteRoleCommandAuthorizer>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandAuthorizer<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            //var assembly = Assembly.GetExecutingAssembly();
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            //services.AddMediatrPipelineBehaviors();
            //services.AddValidatorsFromAssembly(assembly);
            return services;
        }
    }
}
