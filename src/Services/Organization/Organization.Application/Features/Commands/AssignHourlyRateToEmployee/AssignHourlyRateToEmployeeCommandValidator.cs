using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.AssignHourlyRateToEmployee
{
    public class AssignHourlyRateToEmployeeCommandValidator : AbstractValidator<AssignHourlyRateToEmployeeCommand>
    {
        public AssignHourlyRateToEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            // Die HourlyRateId darf null sein, daher keine NotEmpty-Regel hier.
        }
    }
}
