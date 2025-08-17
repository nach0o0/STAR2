using MediatR;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAllScopesForUser
{
    public class GetAllScopesForUserQueryHandler : IRequestHandler<GetAllScopesForUserQuery, List<string>>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;

        public GetAllScopesForUserQueryHandler(IUserPermissionAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<List<string>> Handle(GetAllScopesForUserQuery request, CancellationToken cancellationToken)
        {
            // Delegiert die Logik direkt an die neue Repository-Methode.
            return await _assignmentRepository.GetScopesForUserAsync(request.UserId, cancellationToken);
        }
    }
}
