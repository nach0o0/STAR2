using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeGroupByDateRange
{
    public class GetAttendanceEntriesForEmployeeGroupByDateRangeQueryValidator : AbstractValidator<GetAttendanceEntriesForEmployeeGroupByDateRangeQuery>
    {
        public GetAttendanceEntriesForEmployeeGroupByDateRangeQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("EndDate must be on or after StartDate.");
        }
    }
}
