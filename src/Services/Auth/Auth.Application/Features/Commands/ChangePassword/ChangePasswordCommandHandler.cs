using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IUserContext userContext,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _userContext = userContext;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnauthorizedAccessException();

            var user = await _userRepository.GetByIdAsync(currentUser.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), currentUser.UserId);
            }

            // 1. Prüfe, ob das alte Passwort korrekt ist.
            if (!_passwordHasher.VerifyPassword(command.OldPassword, user.PasswordHash))
            {
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "OldPassword", new[] { "The old password is incorrect." } }
            });
            }

            // 2. Hashe das neue Passwort und aktualisiere die Entität.
            var newPasswordHash = _passwordHasher.HashPassword(command.NewPassword);
            user.ChangePassword(newPasswordHash);

            // Die UnitOfWork-Pipeline speichert die Änderung.
            // Ein 'UserPasswordChangedEvent' sollte in der 'ChangePassword'-Methode ausgelöst werden.
        }
    }
}
