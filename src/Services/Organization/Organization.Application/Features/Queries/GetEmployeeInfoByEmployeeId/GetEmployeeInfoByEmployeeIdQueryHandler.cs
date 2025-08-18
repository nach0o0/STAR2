using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeInfoByEmployeeId
{
    public class GetEmployeeInfoByEmployeeIdQueryHandler
        : IRequestHandler<GetEmployeeInfoByEmployeeIdQuery, GetEmployeeInfoByEmployeeIdQueryResult?>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeInfoByEmployeeIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeInfoByEmployeeIdQueryResult?> Handle(
            GetEmployeeInfoByEmployeeIdQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdWithGroupsAsync(request.EmployeeId, cancellationToken);
            if (employee is null)
            {
                return null;
            }

            var groupIds = employee.EmployeeGroupLinks.Select(l => l.EmployeeGroupId).ToList();

            return new GetEmployeeInfoByEmployeeIdQueryResult(
                employee.Id,
                employee.OrganizationId,
                groupIds);
        }
    }
}
