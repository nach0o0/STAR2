using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteAttendanceEntry
{
    public class DeleteAttendanceEntryCommandValidator : AbstractValidator<DeleteAttendanceEntryCommand>
    {
        public DeleteAttendanceEntryCommandValidator()
        {
            RuleFor(x => x.AttendanceEntryId).NotEmpty();
        }
    }
}
