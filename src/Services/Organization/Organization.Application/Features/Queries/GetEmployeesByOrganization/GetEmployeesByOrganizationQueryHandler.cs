using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByOrganization
{
    public class GetEmployeesByOrganizationQueryHandler
        : IRequestHandler<GetEmployeesByOrganizationQuery, List<GetEmployeesByOrganizationQueryResult>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesByOrganizationQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<GetEmployeesByOrganizationQueryResult>> Handle(
            GetEmployeesByOrganizationQuery request,
            CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetByOrganizationIdAsync(request.OrganizationId, cancellationToken);

            var result = employees
                .Select(e => new GetEmployeesByOrganizationQueryResult(
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.UserId))
                .ToList();

            return result;
        }
    }
}
