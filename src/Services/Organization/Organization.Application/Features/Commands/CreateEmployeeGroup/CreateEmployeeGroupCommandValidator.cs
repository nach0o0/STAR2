using FluentValidation;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateEmployeeGroup
{
    public class CreateEmployeeGroupCommandValidator : AbstractValidator<CreateEmployeeGroupCommand>
    {
        public CreateEmployeeGroupCommandValidator(IEmployeeGroupRepository employeeGroupRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (command, name, cancellation) =>
                    !await employeeGroupRepository.NameExistsInOrganizationAsync(name, command.LeadingOrganizationId, cancellation))
                .WithMessage("An employee group with this name already exists in the organization.");

            RuleFor(x => x.LeadingOrganizationId).NotEmpty();
        }
    }
}
