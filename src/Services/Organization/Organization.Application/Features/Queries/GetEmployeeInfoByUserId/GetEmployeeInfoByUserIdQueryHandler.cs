using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeInfoByUserId
{
    public class GetEmployeeInfoByUserIdQueryHandler
        : IRequestHandler<GetEmployeeInfoByUserIdQuery, GetEmployeeInfoByUserIdQueryResult?>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeInfoByUserIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeInfoByUserIdQueryResult?> Handle(
            GetEmployeeInfoByUserIdQuery query,
            CancellationToken cancellationToken)
        {
            // Lädt den Mitarbeiter inklusive seiner Gruppen-Links.
            var employee = await _employeeRepository.GetByUserIdWithGroupsAsync(query.UserId, cancellationToken);
            if (employee is null)
            {
                return null;
            }

            var groupIds = employee.EmployeeGroupLinks.Select(l => l.EmployeeGroupId).ToList();

            return new GetEmployeeInfoByUserIdQueryResult(
                employee.Id,
                employee.OrganizationId,
                groupIds);
        }
    }
}
