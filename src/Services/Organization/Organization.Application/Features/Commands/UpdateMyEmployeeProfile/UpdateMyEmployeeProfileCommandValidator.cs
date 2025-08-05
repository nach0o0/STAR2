using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateMyEmployeeProfile
{
    public class UpdateMyEmployeeProfileCommandValidator : AbstractValidator<UpdateMyEmployeeProfileCommand>
    {
        public UpdateMyEmployeeProfileCommandValidator()
        {
            // Stellt sicher, dass mindestens ein Feld zum Aktualisieren angegeben wird.
            RuleFor(x => x)
                .Must(x => x.FirstName is not null || x.LastName is not null)
                .WithMessage("At least one field (FirstName or LastName) must be provided for an update.");
        }
    }
}
