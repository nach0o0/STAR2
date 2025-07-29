using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Exceptions;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RemoveEmployeeFromGroup
{
    public class RemoveEmployeeFromGroupCommandHandler : IRequestHandler<RemoveEmployeeFromGroupCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public RemoveEmployeeFromGroupCommandHandler(
            IEmployeeRepository employeeRepository,
            IOrganizationRepository organizationRepository,
            IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeRepository = employeeRepository;
            _organizationRepository = organizationRepository;
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task Handle(RemoveEmployeeFromGroupCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdWithGroupsAsync(command.EmployeeId, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            var group = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (group is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            // Lade die Organisation, zu der die Gruppe gehört.
            var groupOrganization = await _organizationRepository.GetByIdAsync(group.LeadingOrganizationId, cancellationToken);
            if (groupOrganization is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Organization), group.LeadingOrganizationId);
            }

            // Übergib das gesamte Organization-Objekt an die Methode der Employee-Entität.
            employee.UnassignFromGroup(command.EmployeeGroupId, groupOrganization);
        }
    }
}
