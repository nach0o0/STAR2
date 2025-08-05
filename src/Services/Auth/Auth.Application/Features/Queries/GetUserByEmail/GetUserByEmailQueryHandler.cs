using Auth.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailQueryResult?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<GetUserByEmailQueryResult?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(query.Email, cancellationToken);
            if (user is null) return null;

            return new GetUserByEmailQueryResult(user.Id, user.Email);
        }
    }
}
