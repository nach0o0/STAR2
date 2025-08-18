using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateEmployeeWorkModelAssignment
{
    public class UpdateEmployeeWorkModelAssignmentCommandValidator : AbstractValidator<UpdateEmployeeWorkModelAssignmentCommand>
    {
        public UpdateEmployeeWorkModelAssignmentCommandValidator()
        {
            RuleFor(x => x.AssignmentId).NotEmpty();
            // Stellt sicher, dass mindestens ein Datum zum Aktualisieren vorhanden ist.
            RuleFor(x => x)
                .Must(x => x.ValidFrom.HasValue || x.ValidTo.HasValue)
                .WithMessage("At least one date (ValidFrom or ValidTo) must be provided for an update.");
        }
    }
}
