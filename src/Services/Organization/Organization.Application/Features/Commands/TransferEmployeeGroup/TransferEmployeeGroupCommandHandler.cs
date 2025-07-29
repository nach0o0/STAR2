using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.TransferEmployeeGroup
{
    public class TransferEmployeeGroupCommandHandler : IRequestHandler<TransferEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public TransferEmployeeGroupCommandHandler(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task Handle(TransferEmployeeGroupCommand command, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            // Ruft eine Methode auf der Entität auf, um die führende Organisation zu ändern.
            employeeGroup.TransferToOrganization(command.DestinationOrganizationId);
        }
    }
}
