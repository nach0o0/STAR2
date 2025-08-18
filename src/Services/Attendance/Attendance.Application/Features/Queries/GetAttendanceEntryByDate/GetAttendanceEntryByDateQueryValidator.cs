using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntryByDate
{
    public class GetAttendanceEntryByDateQueryValidator : AbstractValidator<GetAttendanceEntryByDateQuery>
    {
        public GetAttendanceEntryByDateQueryValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}
