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
        : IRequestHandler<GetEmployeeInfoByUserIdQuery, (Guid EmployeeId, Guid OrganizationId, List<Guid> EmployeeGroupIds)?>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeInfoByUserIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<(Guid EmployeeId, Guid OrganizationId, List<Guid> EmployeeGroupIds)?> Handle(
            GetEmployeeInfoByUserIdQuery query,
            CancellationToken cancellationToken)
        {
            // Lädt den Mitarbeiter inklusive seiner Gruppen-Links.
            var employee = await _employeeRepository.GetByUserIdWithGroupsAsync(query.UserId, cancellationToken);

            if (employee is null || !employee.OrganizationId.HasValue)
            {
                return null;
            }

            var groupIds = employee.EmployeeGroupLinks.Select(l => l.EmployeeGroupId).ToList();

            // Gibt die rohen Daten als Tuple zurück.
            return (employee.Id, employee.OrganizationId.Value, groupIds);
        }
    }
}
