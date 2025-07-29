using Auth.Application.Interfaces.Infrastructure;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            // 1. Passwort hashen (konkrete Implementierung mit BCrypt liegt in Infrastructure)
            var hashedPassword = _passwordHasher.HashPassword(command.Password);

            // 2. Neue User-Entität erstellen
            var user = new User(command.Email, hashedPassword);

            // 3. User zum Repository hinzufügen (wird nur im Speicher vorgemerkt)
            await _userRepository.AddAsync(user, cancellationToken);

            // 4. ID zurückgeben. Das Speichern (`SaveChanges`) übernimmt die
            //    später implementierte MediatR-Transaktions-Pipeline automatisch.
            return user.Id;
        }
    }
}
