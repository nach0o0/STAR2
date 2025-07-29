using FluentValidation;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.TransferEmployeeGroup
{
    public class TransferEmployeeGroupCommandValidator : AbstractValidator<TransferEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public TransferEmployeeGroupCommandValidator(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;

            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.DestinationOrganizationId).NotEmpty();

            // Stellt sicher, dass in der Ziel-Organisation keine Gruppe mit demselben Namen existiert.
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var groupToTransfer = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellation);
                    if (groupToTransfer is null)
                    {
                        // Dieser Fall wird vom Authorizer/Handler abgefangen, aber zur Sicherheit.
                        return true;
                    }

                    return !await _employeeGroupRepository.NameExistsInOrganizationAsync(
                        groupToTransfer.Name,
                        command.DestinationOrganizationId,
                        cancellation);
                })
                .WithMessage("An employee group with this name already exists in the destination organization.");
        }
    }
}
