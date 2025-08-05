using MediatR;
using Session.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.RevokeSession
{
    public class RevokeSessionCommandHandler : IRequestHandler<RevokeSessionCommand>
    {
        private readonly IActiveSessionRepository _sessionRepository;
        private readonly IUserContext _userContext;

        public RevokeSessionCommandHandler(IActiveSessionRepository sessionRepository, IUserContext userContext)
        {
            _sessionRepository = sessionRepository;
            _userContext = userContext;
        }

        public async Task Handle(RevokeSessionCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;
            var selector = command.RefreshToken.Split(':')[0];

            var session = await _sessionRepository.GetBySelectorAsync(selector, cancellationToken);
            if (session is null)
            {
                // Sitzung existiert nicht, nichts zu tun.
                return;
            }

            // WICHTIGE PRÜFUNG: Stellt sicher, dass ein Benutzer nur seine eigene Sitzung beenden kann.
            if (session.UserId != currentUser.UserId)
            {
                throw new ForbiddenAccessException("You are not authorized to revoke this session.");
            }

            _sessionRepository.Delete(session);
        }
    }
}
