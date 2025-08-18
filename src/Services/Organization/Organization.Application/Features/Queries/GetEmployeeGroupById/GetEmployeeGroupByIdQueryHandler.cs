using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupById
{
    public class GetEmployeeGroupByIdQueryHandler
        : IRequestHandler<GetEmployeeGroupByIdQuery, GetEmployeeGroupByIdQueryResult?>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public GetEmployeeGroupByIdQueryHandler(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task<GetEmployeeGroupByIdQueryResult?> Handle(
            GetEmployeeGroupByIdQuery request,
            CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(request.EmployeeGroupId, cancellationToken);

            if (employeeGroup is null)
            {
                return null;
            }

            return new GetEmployeeGroupByIdQueryResult(
                employeeGroup.Id,
                employeeGroup.Name,
                employeeGroup.LeadingOrganizationId
            );
        }
    }
}
