using FluentValidation;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateOrganization
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationCommandValidator(IOrganizationRepository orgRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .MustAsync(async (command, name, cancellation) =>
                    !await orgRepository.NameExistsAsync(name, command.ParentOrganizationId, cancellation))
                .WithMessage("An organization with this name already exists at this level.");

            RuleFor(x => x.Abbreviation)
                .NotEmpty().WithMessage("Abbreviation is required.")
                .MaximumLength(10).WithMessage("Abbreviation must not exceed 10 characters.");
        }
    }
}
