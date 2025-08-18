using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateOrUpdateAttendanceEntry
{
    public class CreateOrUpdateAttendanceEntryCommandValidator : AbstractValidator<CreateOrUpdateAttendanceEntryCommand>
    {
        public CreateOrUpdateAttendanceEntryCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.AttendanceTypeId).NotEmpty();
            RuleFor(x => x.Note).MaximumLength(500);
        }
    }
}
