using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQueryHandler
        : IRequestHandler<GetOrganizationByIdQuery, GetOrganizationByIdQueryResult?>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetOrganizationByIdQueryHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<GetOrganizationByIdQueryResult?> Handle(
            GetOrganizationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken);

            if (organization is null)
            {
                return null;
            }

            return new GetOrganizationByIdQueryResult(
                organization.Id,
                organization.Name,
                organization.Abbreviation,
                organization.ParentOrganizationId,
                organization.DefaultEmployeeGroupId
            );
        }
    }
}
