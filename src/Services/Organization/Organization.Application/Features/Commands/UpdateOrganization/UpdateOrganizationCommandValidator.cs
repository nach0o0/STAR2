using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
    {
        public UpdateOrganizationCommandValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();

            // Stellt sicher, dass WENN ein Name übergeben wird, dieser nicht leer ist.
            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => x.Name is not null);

            // Stellt sicher, dass WENN eine Abkürzung übergeben wird, diese nicht leer ist.
            RuleFor(x => x.Abbreviation)
                .NotEmpty()
                .When(x => x.Abbreviation is not null);
        }
    }
}
