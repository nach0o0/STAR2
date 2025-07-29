using FluentValidation;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateEmployeeGroup
{
    public class UpdateEmployeeGroupCommandValidator : AbstractValidator<UpdateEmployeeGroupCommand>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public UpdateEmployeeGroupCommandValidator(IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeGroupRepository = employeeGroupRepository;

            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();

            // Stellt sicher, dass der neue Name nicht bereits von einer anderen Gruppe
            // in derselben Organisation verwendet wird.
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var group = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellation);
                    if (group is null) return true; // Wird vom Authorizer/Handler abgefangen

                    // Wenn der Name sich nicht geändert hat, ist die Prüfung erfolgreich.
                    if (group.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return !await _employeeGroupRepository.NameExistsInOrganizationAsync(command.Name, group.LeadingOrganizationId, cancellation);
                })
                .WithMessage("An employee group with this name already exists in the organization.");
        }
    }
}
