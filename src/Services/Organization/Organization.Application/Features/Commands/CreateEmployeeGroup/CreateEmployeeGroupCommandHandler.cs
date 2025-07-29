using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateEmployeeGroup
{
    public class CreateEmployeeGroupCommandHandler : IRequestHandler<CreateEmployeeGroupCommand, Guid>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public CreateEmployeeGroupCommandHandler(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task<Guid> Handle(CreateEmployeeGroupCommand command, CancellationToken cancellationToken)
        {
            var employeeGroup = new EmployeeGroup(command.Name, command.LeadingOrganizationId);
            await _employeeGroupRepository.AddAsync(employeeGroup, cancellationToken);
            return employeeGroup.Id;
        }
    }
}
