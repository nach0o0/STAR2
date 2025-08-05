using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using MediatR;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetMyProfile
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, GetMyProfileQueryResult>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public GetMyProfileQueryHandler(IUserContext userContext, IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<GetMyProfileQueryResult> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnauthorizedAccessException();

            var user = await _userRepository.GetByIdAsync(currentUser.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), currentUser.UserId);
            }

            var result = new GetMyProfileQueryResult(user.Id, user.Email);
            return result;
        }
    }
}
