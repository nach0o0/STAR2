using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserContext _userContext;

        public AuthorizationBehavior(IServiceProvider serviceProvider, IUserContext userContext)
        {
            _serviceProvider = serviceProvider;
            _userContext = userContext;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Finde den spezifischen Authorizer-Typ für den aktuellen Command.
            var authorizerType = typeof(ICommandAuthorizer<>).MakeGenericType(request.GetType());

            // Hole den Authorizer aus dem DI-Container.
            var authorizer = _serviceProvider.GetService(authorizerType);

            if (authorizer is not null)
            {
                // Hole den Benutzerkontext. Dieser kann null sein.
                var currentUser = _userContext.GetCurrentUser();

                // Führe die Autorisierung NUR DANN aus, wenn ein Benutzerkontext existiert.
                // (Also bei einem API-Aufruf)
                if (currentUser is not null)
                {
                    await ((dynamic)authorizer).AuthorizeAsync((dynamic)request, currentUser, cancellationToken);
                }
                // Wenn currentUser null ist (Aufruf aus einem Event Handler), wird die Prüfung
                // einfach übersprungen, da es ein vertrauenswürdiger Systemaufruf ist.
            }

            return await next();
        }
    }
}
