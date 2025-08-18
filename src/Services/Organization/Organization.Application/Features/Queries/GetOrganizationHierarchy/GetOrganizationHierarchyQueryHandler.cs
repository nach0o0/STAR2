using MassTransit;
using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationHierarchy
{
    public class GetOrganizationHierarchyQueryHandler
        : IRequestHandler<GetOrganizationHierarchyQuery, GetOrganizationHierarchyQueryResult?>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetOrganizationHierarchyQueryHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<GetOrganizationHierarchyQueryResult?> Handle(
            GetOrganizationHierarchyQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Lade die Wurzel-Organisation der Hierarchie
            var rootOrganization = await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken);

            if (rootOrganization is null)
            {
                return null;
            }

            // 2. Starte den rekursiven Mapping-Prozess
            return await MapToQueryResultAsync(rootOrganization, cancellationToken);
        }

        private async Task<GetOrganizationHierarchyQueryResult> MapToQueryResultAsync(
            Organization.Domain.Entities.Organization organization,
            CancellationToken cancellationToken)
        {
            // Hole die direkten untergeordneten Organisationen über das Repository
            var childrenEntities = await _organizationRepository.GetSubOrganizationsAsync(organization.Id, cancellationToken);

            var childrenResults = new List<GetOrganizationHierarchyQueryResult>();
            foreach (var child in childrenEntities)
            {
                // Rekursiver Aufruf, um die Hierarchie für jedes Kind aufzubauen
                childrenResults.Add(await MapToQueryResultAsync(child, cancellationToken));
            }

            return new GetOrganizationHierarchyQueryResult(
                organization.Id,
                organization.Name,
                organization.Abbreviation,
                childrenResults
            );
        }
    }
}
