using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupsForEmployee
{
    public class GetEmployeeGroupsForEmployeeQueryHandler
        : IRequestHandler<GetEmployeeGroupsForEmployeeQuery, List<GetEmployeeGroupsForEmployeeQueryResult>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public GetEmployeeGroupsForEmployeeQueryHandler(
            IEmployeeRepository employeeRepository,
            IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task<List<GetEmployeeGroupsForEmployeeQueryResult>> Handle(
            GetEmployeeGroupsForEmployeeQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdWithGroupsAsync(request.EmployeeId, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Employee), request.EmployeeId);
            }

            var groupIds = employee.EmployeeGroupLinks.Select(l => l.EmployeeGroupId).ToList();
            if (!groupIds.Any())
            {
                return new List<GetEmployeeGroupsForEmployeeQueryResult>();
            }

            var results = new List<GetEmployeeGroupsForEmployeeQueryResult>();
            foreach (var groupId in groupIds)
            {
                var group = await _employeeGroupRepository.GetByIdAsync(groupId, cancellationToken);
                if (group != null)
                {
                    results.Add(new GetEmployeeGroupsForEmployeeQueryResult(group.Id, group.Name));
                }
            }

            return results;
        }
    }
}
