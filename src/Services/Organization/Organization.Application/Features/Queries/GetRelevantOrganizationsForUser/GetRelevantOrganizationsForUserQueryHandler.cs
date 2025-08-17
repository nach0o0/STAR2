using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetRelevantOrganizationsForUser
{
    public class GetRelevantOrganizationsForUserQueryHandler : IRequestHandler<GetRelevantOrganizationsForUserQuery, List<GetRelevantOrganizationsForUserQueryResult>>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserContext _userContext;

        public GetRelevantOrganizationsForUserQueryHandler(IOrganizationRepository organizationRepository, IUserContext userContext)
        {
            _organizationRepository = organizationRepository;
            _userContext = userContext;
        }

        public async Task<List<GetRelevantOrganizationsForUserQueryResult>> Handle(GetRelevantOrganizationsForUserQuery request, CancellationToken ct)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // 1. Extrahiere alle Organization-IDs aus den Scopes des Benutzers.
            var orgIdsFromScopes = currentUser.PermissionsByScope.Keys
                .Where(scope => scope.StartsWith($"{PermittedScopeTypes.Organization}:"))
                .Select(scope => Guid.Parse(scope.Split(':')[1]))
                .ToList();

            // 2. Füge die primäre Organisation des Benutzers hinzu (falls vorhanden).
            if (currentUser.OrganizationId.HasValue)
            {
                orgIdsFromScopes.Add(currentUser.OrganizationId.Value);
            }

            if (!orgIdsFromScopes.Any())
            {
                return new List<GetRelevantOrganizationsForUserQueryResult>();
            }

            // 3. Entferne Duplikate und hole alle relevanten Organisationen aus der DB.
            var distinctOrgIds = orgIdsFromScopes.Distinct().ToList();
            var organizations = await _organizationRepository.GetByIdsAsync(distinctOrgIds, ct);

            // 4. Wandle das Ergebnis um und setze das `IsPrimary`-Flag.
            var result = organizations.Select(org => new GetRelevantOrganizationsForUserQueryResult(
                org.Id,
                org.Name,
                org.Id == currentUser.OrganizationId
            )).ToList();

            return result;
        }
    }
}
