using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateEmployeeGroup
{
    public class UpdateEmployeeGroupCommandHandler : IRequestHandler<UpdateEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public UpdateEmployeeGroupCommandHandler(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task Handle(UpdateEmployeeGroupCommand command, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            employeeGroup.UpdateName(command.Name);
        }
    }
}
