using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByEmployeeGroup
{
    public class GetEmployeesByEmployeeGroupQueryHandler : IRequestHandler<GetEmployeesByEmployeeGroupQuery, List<GetEmployeesByEmployeeGroupQueryResult>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesByEmployeeGroupQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<GetEmployeesByEmployeeGroupQueryResult>> Handle(GetEmployeesByEmployeeGroupQuery request, CancellationToken ct)
        {
            var employees = await _employeeRepository.GetByEmployeeGroupIdAsync(request.EmployeeGroupId, ct);
            return employees.Select(e => new GetEmployeesByEmployeeGroupQueryResult(
                e.Id,
                e.FirstName,
                e.LastName,
                e.UserId,
                e.OrganizationId
            )).ToList();
        }
    }
}
