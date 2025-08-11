using MediatR;
using Shared.Application.Interfaces.Clients;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.FindUserByEmail
{
    public class FindUserByEmailCommandHandler : IRequestHandler<FindUserByEmailCommand, FindUserByEmailResult>
    {
        private readonly IAuthServiceClient _authServiceClient;

        public FindUserByEmailCommandHandler(IAuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        public async Task<FindUserByEmailResult> Handle(FindUserByEmailCommand command, CancellationToken ct)
        {
            var userResponse = await _authServiceClient.GetUserByEmailAsync(command.Email, ct);
            if (userResponse == null)
            {
                throw new NotFoundException("User", command.Email);
            }

            var (userId, email, firstName, lastName) = userResponse.Value;

            return new FindUserByEmailResult(userId, email, firstName, lastName);
        }
    }
}
