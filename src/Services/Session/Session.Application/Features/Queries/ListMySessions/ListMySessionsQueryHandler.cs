using MediatR;
using Session.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Queries.ListMySessions
{
    public class ListMySessionsQueryHandler : IRequestHandler<ListMySessionsQuery, List<ListMySessionsQueryResult>>
    {
        private readonly IActiveSessionRepository _sessionRepository;
        private readonly IUserContext _userContext;

        public ListMySessionsQueryHandler(IActiveSessionRepository sessionRepository, IUserContext userContext)
        {
            _sessionRepository = sessionRepository;
            _userContext = userContext;
        }

        public async Task<List<ListMySessionsQueryResult>> Handle(ListMySessionsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnauthorizedAccessException();

            var sessions = await _sessionRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);

            return sessions
                .Select(s => new ListMySessionsQueryResult(s.Id, s.CreatedAt, s.ClientInfo))
                .ToList();
        }
    }
}
