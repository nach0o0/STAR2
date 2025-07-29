using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteEmployeeGroup
{
    public class DeleteEmployeeGroupCommandHandler : IRequestHandler<DeleteEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public DeleteEmployeeGroupCommandHandler(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task Handle(DeleteEmployeeGroupCommand command, CancellationToken cancellationToken)
        {
            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            employeeGroup.PrepareForDeletion();
            _employeeGroupRepository.Delete(employeeGroup);
        }
    }
}
