using Auth.Application.Interfaces.Infrastructure;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Domain.Exceptions;

namespace Auth.Application.Features.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IBasicTokenService _tokenService;

        public LoginQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IBasicTokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            // 1. User finden
            var user = await _userRepository.GetByEmailAsync(query.Email, cancellationToken);
            if (user is null || !user.IsActive)
            {
                throw new NotFoundException(nameof(User), query.Email);
            }

            // 2. Passwort verifizieren
            var passwordIsValid = _passwordHasher.VerifyPassword(query.Password, user.PasswordHash);
            if (!passwordIsValid)
            {
                throw new NotFoundException(nameof(User), query.Email);
            }

            // 3. Basis-Token generieren und zurückgeben
            var token = _tokenService.GenerateToken(user);

            return token;
        }
    }
}
