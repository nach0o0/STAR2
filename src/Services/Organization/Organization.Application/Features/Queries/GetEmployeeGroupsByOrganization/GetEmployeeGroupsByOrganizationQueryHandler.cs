using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupsByOrganization
{
    public class GetEmployeeGroupsByOrganizationQueryHandler
        : IRequestHandler<GetEmployeeGroupsByOrganizationQuery, List<GetEmployeeGroupsByOrganizationQueryResult>>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public GetEmployeeGroupsByOrganizationQueryHandler(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task<List<GetEmployeeGroupsByOrganizationQueryResult>> Handle(
            GetEmployeeGroupsByOrganizationQuery request,
            CancellationToken cancellationToken)
        {
            var employeeGroups = await _employeeGroupRepository.GetByLeadingOrganizationIdAsync(request.OrganizationId, cancellationToken);

            return employeeGroups
                .Select(eg => new GetEmployeeGroupsByOrganizationQueryResult(
                    eg.Id,
                    eg.Name))
                .ToList();
        }
    }
}
