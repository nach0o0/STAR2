using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceTypesByEmployeeGroup
{
    public class GetAttendanceTypesByEmployeeGroupQueryValidator : AbstractValidator<GetAttendanceTypesByEmployeeGroupQuery>
    {
        public GetAttendanceTypesByEmployeeGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
