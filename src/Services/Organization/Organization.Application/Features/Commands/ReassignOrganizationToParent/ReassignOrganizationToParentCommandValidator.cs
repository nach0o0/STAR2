using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.ReassignOrganizationParent
{
    public class ReassignOrganizationToParentCommandValidator : AbstractValidator<ReassignOrganizationToParentCommand>
    {
        public ReassignOrganizationToParentCommandValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();

            // Stellt sicher, dass eine Organisation nicht ihr eigener Parent wird.
            RuleFor(x => x)
                .Must(command => command.OrganizationId != command.NewParentId)
                .WithMessage("An organization cannot be its own parent.");
        }
    }
}
