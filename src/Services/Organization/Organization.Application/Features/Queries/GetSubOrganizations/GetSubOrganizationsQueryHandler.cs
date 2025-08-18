using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetSubOrganizations
{
    public class GetSubOrganizationsQueryHandler
        : IRequestHandler<GetSubOrganizationsQuery, List<GetSubOrganizationsQueryResult>>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetSubOrganizationsQueryHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<List<GetSubOrganizationsQueryResult>> Handle(
            GetSubOrganizationsQuery request,
            CancellationToken cancellationToken)
        {
            var subOrganizations = await _organizationRepository.GetSubOrganizationsAsync(request.ParentOrganizationId, cancellationToken);

            return subOrganizations
                .Select(org => new GetSubOrganizationsQueryResult(
                    org.Id,
                    org.Name,
                    org.Abbreviation))
                .ToList();
        }
    }
}
