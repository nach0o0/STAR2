using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateAttendanceType
{
    public class CreateAttendanceTypeCommandValidator : AbstractValidator<CreateAttendanceTypeCommand>
    {
        public CreateAttendanceTypeCommandValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Abbreviation).NotEmpty().MaximumLength(5);
            RuleFor(x => x.Color).NotEmpty().Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .WithMessage("Color must be a valid hex color code (e.g., #FF5733).");
        }
    }
}
