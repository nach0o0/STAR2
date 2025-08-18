using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.CalculateTargetWorkingHours
{
    public class CalculateTargetWorkingHoursQueryValidator : AbstractValidator<CalculateTargetWorkingHoursQuery>
    {
        public CalculateTargetWorkingHoursQueryValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
}
