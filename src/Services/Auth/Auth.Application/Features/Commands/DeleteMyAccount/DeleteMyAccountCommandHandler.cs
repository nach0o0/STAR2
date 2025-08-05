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

namespace Auth.Application.Features.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommandHandler : IRequestHandler<DeleteMyAccountCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        private readonly IPasswordHasher _passwordHasher;

        public DeleteMyAccountCommandHandler(
            IUserRepository userRepository,
            IUserContext userContext,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _userContext = userContext;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(DeleteMyAccountCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnauthorizedAccessException();

            var user = await _userRepository.GetByIdAsync(currentUser.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), currentUser.UserId);
            }

            if (!_passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
            {
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Password", new[] { "The password is incorrect." } }
            });
            }

            user.PrepareForDeletion();
            _userRepository.Delete(user);
        }
    }
}
