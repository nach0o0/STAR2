using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByUserIds
{
    public class GetEmployeesByUserIdsQueryHandler : IRequestHandler<GetEmployeesByUserIdsQuery, List<GetEmployeesByUserIdsQueryResult>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public GetEmployeesByUserIdsQueryHandler(IEmployeeRepository employeeRepository) => _employeeRepository = employeeRepository;

        public async Task<List<GetEmployeesByUserIdsQueryResult>> Handle(GetEmployeesByUserIdsQuery request, CancellationToken ct)
        {
            var employees = await _employeeRepository.GetByUserIdsAsync(request.UserIds, ct);
            return employees.Select(e => new GetEmployeesByUserIdsQueryResult(
                e.UserId!.Value, e.Id, e.FirstName, e.LastName
            )).ToList();
        }
    }
}
