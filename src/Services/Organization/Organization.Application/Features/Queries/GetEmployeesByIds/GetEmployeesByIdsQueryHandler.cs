using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByIds
{
    public class GetEmployeesByIdsQueryHandler : IRequestHandler<GetEmployeesByIdsQuery, List<GetEmployeesByIdsQueryResult>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesByIdsQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<GetEmployeesByIdsQueryResult>> Handle(GetEmployeesByIdsQuery query, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetByIdsAsync(query.EmployeeIds, cancellationToken);

            return employees.Select(e => new GetEmployeesByIdsQueryResult(
                e.Id,
                e.FirstName,
                e.LastName
            )).ToList();
        }
    }
}
