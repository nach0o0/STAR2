using Auth.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdQueryResult?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);

            if (user is null)
            {
                return null;
            }

            return new GetUserByIdQueryResult(user.Id, user.Email);
        }
    }
}
