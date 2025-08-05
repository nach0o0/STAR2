using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.PrivilegedResetPassword
{
    public class PrivilegedResetPasswordCommandHandler : IRequestHandler<PrivilegedResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public PrivilegedResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(PrivilegedResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), command.UserId);
            }

            var newPasswordHash = _passwordHasher.HashPassword(command.NewPassword);
            user.ChangePassword(newPasswordHash);
        }
    }
}
