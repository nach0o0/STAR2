using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetTopLevelOrganizations
{
    public class GetTopLevelOrganizationsQueryHandler
        : IRequestHandler<GetTopLevelOrganizationsQuery, List<GetTopLevelOrganizationsQueryResult>>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetTopLevelOrganizationsQueryHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<List<GetTopLevelOrganizationsQueryResult>> Handle(
            GetTopLevelOrganizationsQuery request,
            CancellationToken cancellationToken)
        {
            var organizations = await _organizationRepository.GetTopLevelOrganizationsAsync(cancellationToken);

            return organizations
                .Select(org => new GetTopLevelOrganizationsQueryResult(
                    org.Id,
                    org.Name,
                    org.Abbreviation))
                .ToList();
        }
    }
}
