using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateAttendanceType
{
    public class UpdateAttendanceTypeCommandValidator : AbstractValidator<UpdateAttendanceTypeCommand>
    {
        public UpdateAttendanceTypeCommandValidator()
        {
            RuleFor(x => x.AttendanceTypeId).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(50);
            RuleFor(x => x.Abbreviation).MaximumLength(5);
            RuleFor(x => x.Color).Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .WithMessage("Color must be a valid hex color code (e.g., #FF5733).")
                .When(x => x.Color is not null);
        }
    }
}
